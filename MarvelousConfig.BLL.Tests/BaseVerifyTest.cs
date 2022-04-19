﻿using Auth.BusinessLayer.Helpers;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using System;
using static Moq.It;

namespace MarvelousConfigs.BLL.Tests
{
    public class BaseVerifyTest<T>
    {
        protected Mock<ILogger<T>> _logger;

        protected void VerifyLogger(LogLevel level, int times)
        {
            _logger.Verify(x => x.Log(level,
                    It.IsAny<EventId>(),
                    It.Is<IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<IsAnyType, Exception, string>>()!),
                Times.Exactly(times));
        }

        protected static void VerifyRequestTests(Mock<IRestClient> client)
        {
            client.Verify(x => x.AddMicroservice(Microservice.MarvelousConfigs), Times.Once);
            client.Verify(x => x.ExecuteAsync<string>(IsAny<RestRequest>(), default), Times.Once);
        }
    }
}
