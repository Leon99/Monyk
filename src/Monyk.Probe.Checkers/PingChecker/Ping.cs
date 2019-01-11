using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Monyk.Probe.Checkers.PingChecker
{
    public interface IPing
    {
        Task<PingReply> SendAsync(string address);
    }

    public class PingReply
    {
        public IPStatus Status { get;set; }
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
                Status = reply.Status
            };
        }
    }
}
