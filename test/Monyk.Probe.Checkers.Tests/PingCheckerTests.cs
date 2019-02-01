using System.Net.NetworkInformation;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Monyk.Common.Models;
using Moq;
using Xunit;

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
            var pingFactoryMock = new Mock<IPingFactory>();
            pingFactoryMock
                .Setup(factory => factory.Create())
                .Returns(() =>
                {
                    var pingMock = new Mock<IPing>();
                    pingMock
                        .Setup(ping => ping.SendAsync(It.IsAny<string>()))
                        .ReturnsAsync(() => new PingReply
                        {
                            Status = ipStatus
                        });
                    return pingMock.Object;
                });
            var pingChecker = new PingChecker(pingFactoryMock.Object, Mock.Of<ILogger<PingChecker>>());
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
