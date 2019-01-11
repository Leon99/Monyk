using System.Net.NetworkInformation;
using FluentAssertions;
using Monyk.Agent.Checkers;
using Monyk.Agent.Checkers.PingChecker;
using Moq;
using Xunit;
using PingReply = Monyk.Agent.Checkers.PingChecker.PingReply;

namespace Monyk.Agent.Tests
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
            var config = new PingCheckConfig
            {
                Host = "foo"
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
