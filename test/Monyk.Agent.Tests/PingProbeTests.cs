using System.Net.NetworkInformation;
using FluentAssertions;
using Monyk.Agent.Probes;
using Monyk.Agent.Probes.PingProbe;
using Moq;
using Xunit;
using PingReply = Monyk.Agent.Probes.PingProbe.PingReply;

namespace Monyk.Agent.Tests
{
    public class PingProbeTests
    {
        [Theory]
        [InlineData(IPStatus.Success, VerificationResultStatus.Success, null)]
        [InlineData(IPStatus.DestinationHostUnreachable, VerificationResultStatus.Failure, null)]
        public async void RunCheck_BasicScenarios(IPStatus ipStatus, VerificationResultStatus resultStatus, string resultMessage)
        {
            // Arrange
            var pingMock = new Mock<IPing>();
            pingMock
                .Setup(ping => ping.SendAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new PingReply
                {
                    Status = ipStatus
                });
            var probe = new PingProbe(pingMock.Object);
            var config = new PingProbeConfig
            {
                Host = "foo"
            };

            // Act
            var result = await probe.RunVerificationAsync(config);

            // Assert
            result.Should().BeEquivalentTo(new VerificationResult
            {
                Status = resultStatus,
                Message = resultMessage
            });
        }
    }
}
