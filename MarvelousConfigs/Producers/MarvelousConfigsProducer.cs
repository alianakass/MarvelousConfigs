using Marvelous.Contracts.Configurations;
using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using MassTransit;

namespace MarvelousConfigs.API.RMQ.Producers
{
    public class MarvelousConfigsProducer : IMarvelousConfigsProducer
    {
        private readonly IConfigsService _config;
        private readonly ILogger<MarvelousConfigsProducer> _logger;
        private readonly IBus _bus;

        public MarvelousConfigsProducer(IConfigsService configsService, ILogger<MarvelousConfigsProducer> logger, IBus bus)
        {
            _config = configsService;
            _logger = logger;
            _bus = bus;
        }

        public async Task NotifyConfigurationAddedOrUpdated(int id)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            ConfigModel config = new ConfigModel() { Id = 1, Key = "TestKey", Value = "TestValue", ServiceId = 100 };
            _logger.LogInformation($"Try publish config id{id} for {(((Microservice)config.ServiceId).ToString())}");
            //var config = _ await _config.GetConfigById(id);

            await CheckMicroserviceAndPublish(config, source);
            _logger.LogInformation($"Config id{config.Id} for {(((Microservice)config.ServiceId).ToString())} published");
        }

        private async Task CheckMicroserviceAndPublish(ConfigModel config, CancellationTokenSource source)
        {
            switch (config.ServiceId)
            {
                case (int)Microservice.MarvelousAccountChecking:
                    await _bus.Publish<AccountCheckingCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousReporting:
                    await _bus.Publish<ReportingCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousResource:
                    await _bus.Publish<ResourceCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousTransactionStore:
                    await _bus.Publish<TransactionStoreCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousCrm:
                    await _bus.Publish<CrmCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousEmailSendler:
                    await _bus.Publish<EmailSendlerCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousRatesApi:
                    await _bus.Publish<RatesApiCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousAuth:
                    await _bus.Publish<AuthCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case (int)Microservice.MarvelousSmsSendler:
                    await _bus.Publish<SmsSendlerCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                 source.Token);
                    break;

                default:
                    throw new Exception($"Unable to send configurations {config.Id} for {(Microservice)config.ServiceId}");

            }
        }
    }
}
