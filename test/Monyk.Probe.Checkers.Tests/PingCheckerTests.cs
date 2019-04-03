using System.Net;
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
        [InlineData(IPStatus.Success, "127.0.0.1", CheckResultStatus.Success, "Resolved IP address: 127.0.0.1")]
        [InlineData(IPStatus.DestinationHostUnreachable, "0.0.0.0", CheckResultStatus.Failure, "Unable to resolve IP address")]
        public async void RunCheck_BasicScenarios(IPStatus ipStatus, string ipAddress, CheckResultStatus resultStatus, string resultMessage)
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
                            Status = ipStatus,
                            IPAddress = IPAddress.Parse(ipAddress)
                        });
                    return pingMock.Object;
                });
            var pingChecker = new PingChecker(Mock.Of<ILogger<PingChecker>>(), pingFactoryMock.Object);
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
