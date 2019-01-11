using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Monyk.Probe.Checkers.HttpChecker;
using Moq;
using Moq.Protected;
using Xunit;

namespace Monyk.Probe.Checkers.Tests
{
    public class HttpCheckerTests
    {
        [Theory]
        [InlineData(HttpStatusCode.OK, CheckResultStatus.Success, null)]
        [InlineData(HttpStatusCode.BadRequest, CheckResultStatus.Failure, null)]
        public async void RunCheck_BasicScenarios(HttpStatusCode httpStatus, CheckResultStatus resultStatus, string resultMessage)
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatus
                })
                .Verifiable();
            var httpClientFactory = new FakeHttpClientFactory(handlerMock.Object);
            var checker = new HttpChecker.HttpChecker(httpClientFactory);
            var config = new HttpCheckConfig
            {
                Url = new Uri("http://foo.bar/baz")
            };

            // Act
            var result = await checker.RunCheckAsync(config);

            // Assert
            result.Should().BeEquivalentTo(new CheckResult
            {
                Status = resultStatus,
                Description = resultMessage
            });

            var expectedUri = new Uri("http://foo.bar/baz");
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    &&
                    req.RequestUri == expectedUri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
