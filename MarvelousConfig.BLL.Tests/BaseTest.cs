using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace MarvelousConfigs.BLL.Tests
{
    public class BaseTest<T>
    {
        protected Mock<ILogger<T>> _logger;

        protected void VerifyLogger(LogLevel logLevel, String message)
        {
            _logger.Verify(
               x => x.Log(
                   logLevel,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((o, t) =>
                   string.Equals(message, o.ToString(),
                   StringComparison.InvariantCultureIgnoreCase)),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
