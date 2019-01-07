using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Monyk.Agent.Probes;
using Monyk.Agent.Probes.HttpProbe;
using Moq;
using Moq.Protected;
using Xunit;

namespace Monyk.Agent.Tests
{
    public class HttpProbeTests
    {
        [Theory]
        [InlineData(HttpStatusCode.OK, VerificationResultStatus.Success, null)]
        [InlineData(HttpStatusCode.BadRequest, VerificationResultStatus.Failure, null)]
        public async void RunCheck_BasicScenarios(HttpStatusCode httpStatus, VerificationResultStatus resultStatus, string resultMessage)
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
            var probe = new HttpProbe(httpClientFactory);
            var probeConfig = new HttpProbeConfig
            {
                Url = new Uri("http://foo.bar/baz")
            };

            // Act
            var result = await probe.RunVerificationAsync(probeConfig);

            // Assert
            result.Should().BeEquivalentTo(new VerificationResult
            {
                Status = resultStatus,
                Message = resultMessage
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
