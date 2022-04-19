using Auth.BusinessLayer.Helpers;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using static Moq.It;

namespace MarvelousConfigs.API.Tests
{
    public class BaseTest<T>
    {
        protected Mock<ILogger<T>> _logger;

        protected void VerifyLogger(LogLevel level, int times)
        {
            _logger.Verify(v => v.Log(level,
                    It.IsAny<EventId>(),
                    It.Is<IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<IsAnyType, Exception, string>>()!),
                Times.Exactly(times));
        }

        protected static void VerifyRequestHelperTests(Mock<IRestClient> client)
        {
            client.Verify(v => v.AddMicroservice(Microservice.MarvelousConfigs), Times.Once);
            client.Verify(v => v.ExecuteAsync<IEnumerable<string>>(IsAny<RestRequest>(), default), Times.Once);
        }

        //protected void VerifyLogger(LogLevel logLevel, String message)
        //{
        //    _logger.Verify(
        //       x => x.Log(
        //           logLevel,
        //           It.IsAny<EventId>(),
        //           It.Is<It.IsAnyType>((o, t) =>
        //           string.Equals(message, o.ToString(),
        //           StringComparison.InvariantCultureIgnoreCase)),
        //           It.IsAny<Exception>(),
        //           It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        //}
    }
}
