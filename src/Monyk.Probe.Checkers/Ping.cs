using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Monyk.Probe.Checkers
{
    public interface IPingFactory
    {
        IPing Create();
    }

    public class PingFactory : IPingFactory
    {
        public IPing Create()
        {
            return new Ping();
        }
    }

    public interface IPing
    {
        Task<PingReply> SendAsync(string address);
    }

    public class PingReply
    {
        public IPStatus Status { get;set; }
        public TimeSpan RoundtripTime { get; set; }
        public IPAddress IPAddress { get; set; }
    }

    public class Ping : IPing
    {
        private readonly System.Net.NetworkInformation.Ping _ping;

        public Ping()
        {
            _ping = new System.Net.NetworkInformation.Ping();
        }

        public async Task<PingReply> SendAsync(string address)
        {
            var reply = await _ping.SendPingAsync(address);
            return new PingReply
            {
                Status = reply.Status,
                IPAddress = reply.Address,
                RoundtripTime = TimeSpan.FromMilliseconds(reply.RoundtripTime)
            };
        }
    }
}
