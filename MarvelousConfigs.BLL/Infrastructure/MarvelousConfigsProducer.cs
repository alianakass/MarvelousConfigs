using Marvelous.Contracts.Configurations;
using Marvelous.Contracts.EmailMessageModels;
using MarvelousConfigs.DAL.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace MarvelousConfigs.BLL.Infrastructure
{
    public class MarvelousConfigsProducer : IMarvelousConfigsProducer
    {
        private readonly ILogger<MarvelousConfigsProducer> _logger;
        private readonly IBus _bus;

        public MarvelousConfigsProducer(ILogger<MarvelousConfigsProducer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task NotifyAdminAboutErrorToEmail(string mess)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            _logger.LogInformation($"Try publish message about error for service {Marvelous.Contracts.Enums.Microservice.MarvelousEmailSender}");
            await _bus.Publish<EmailErrorMessage>(new
            {
                ServiceName = Marvelous.Contracts.Enums.Microservice.MarvelousConfigs.ToString(),
                TextMessage = mess
            },
               source.Token);
            _logger.LogInformation($"Message about error for service {Marvelous.Contracts.Enums.Microservice.MarvelousEmailSender} published");
        }

        public async Task NotifyConfigurationUpdated(Config config)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            _logger.LogInformation($"Try publish config id{config.Id} for {((Marvelous.Contracts.Enums.Microservice)config.ServiceId)}");

            await CheckMicroserviceAndPublish(config, source);
            _logger.LogInformation($"Config id{config.Id} for {((Marvelous.Contracts.Enums.Microservice)config.ServiceId)} published");
        }

        private async Task CheckMicroserviceAndPublish(Config config, CancellationTokenSource source)
        {
            switch ((Marvelous.Contracts.Enums.Microservice)config.ServiceId)
            {
                case Marvelous.Contracts.Enums.Microservice.MarvelousAccountChecking:
                    await _bus.Publish<AccountCheckingCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Marvelous.Contracts.Enums.Microservice.MarvelousResource:
                    await _bus.Publish<ResourceCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Marvelous.Contracts.Enums.Microservice.MarvelousCrm:
                    await _bus.Publish<CrmCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Marvelous.Contracts.Enums.Microservice.MarvelousEmailSender:
                    await _bus.Publish<EmailSendlerCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Marvelous.Contracts.Enums.Microservice.MarvelousRatesApi:
                    await _bus.Publish<RatesApiCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Marvelous.Contracts.Enums.Microservice.MarvelousAuth:
                    await _bus.Publish<AuthCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Marvelous.Contracts.Enums.Microservice.MarvelousSmsSender:
                    await _bus.Publish<SmsSendlerCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                 source.Token);
                    break;

                default:
                    throw new Exception($"Unable to send configurations {config.Id} for {(Marvelous.Contracts.Enums.Microservice)config.ServiceId}");

            }
        }
    }
}
