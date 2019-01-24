using System.Net.NetworkInformation;
using FluentAssertions;
using Monyk.Common.Models;
using Moq;
using Xunit;
using PingReply = Monyk.Probe.Checkers.PingReply;

namespace Monyk.Probe.Checkers.Tests
{
    public class PingCheckerTests
    {
        [Theory]
        [InlineData(IPStatus.Success, CheckResultStatus.Success, null)]
        [InlineData(IPStatus.DestinationHostUnreachable, CheckResultStatus.Failure, null)]
        public async void RunCheck_BasicScenarios(IPStatus ipStatus, CheckResultStatus resultStatus, string resultMessage)
        {
            // Arrange
            var pingMock = new Mock<IPing>();
            pingMock
                .Setup(ping => ping.SendAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new PingReply
                {
                    Status = ipStatus
                });
            var pingChecker = new PingChecker(pingMock.Object);
            var config = new CheckConfiguration
            {
                Target = "foo"
            };

            // Act
            var result = await pingChecker.RunCheckAsync(config);

            // Assert
            result.Should().BeEquivalentTo(new CheckResult
            {
                Status = resultStatus,
                Description = resultMessage
            });
        }
    }
}
