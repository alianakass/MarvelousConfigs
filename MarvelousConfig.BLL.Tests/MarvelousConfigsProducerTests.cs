using Marvelous.Contracts.Configurations;
using Marvelous.Contracts.EmailMessageModels;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.DAL.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelousConfigs.BLL.Tests
{
    internal class MarvelousConfigsProducerTests : BaseVerifyTest<MarvelousConfigsProducer>
    {
        private Mock<IBus> _bus;
        private MarvelousConfigsProducer _producer;

        [SetUp]
        public void SetUp()
        {
            _bus = new Mock<IBus>();
            _logger = new Mock<ILogger<MarvelousConfigsProducer>>();
            _producer = new MarvelousConfigsProducer(_logger.Object, _bus.Object);
        }

        [Test]
        public async Task NotifyAdminAboutErrorToEmailTest_ValidRequestReceived_ShouldPublishMessage()
        {
            //given
            string message = "test";
            _bus.Setup(x => x.Publish(It.IsAny<EmailErrorMessage>(), new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token));

            //when
            await _producer.NotifyAdminAboutErrorToEmail(message);

            //then
            _bus.Verify(v => v.Publish(It.IsAny<EmailErrorMessage>(), new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCase(3)]
        public async Task NotifyConfigurationUpdatedTest(int id)
        {
            //given
            Config cfg = new Config() { Id = 3, Created = DateTime.Now, Key = "Key", Value = "Value", ServiceId = 9 };
            _bus.Setup(x => x.Publish(It.IsAny<AuthCfg>(), new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token));

            //when
            await _producer.NotifyConfigurationUpdated(cfg);

            //then
            _bus.Verify(v => v.Publish(It.IsAny<AuthCfg>(), new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCase(3)]
        public async Task NotifyConfigurationUpdatedTest_WhenMicroserviceNotFound_ShouldThrowException(int id)
        {
            //given
            Config cfg = new Config() { Id = 3, Created = DateTime.Now, Key = "Key", Value = "Value", ServiceId = 100000 };
            _bus.Setup(x => x.Publish(It.IsAny<AuthCfg>(), new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token));

            //when       

            //then
            Assert.ThrowsAsync<Exception>(async () => await _producer.NotifyConfigurationUpdated(cfg));
            _bus.Verify(v => v.Publish(It.IsAny<AuthCfg>(), new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }
    }
}
