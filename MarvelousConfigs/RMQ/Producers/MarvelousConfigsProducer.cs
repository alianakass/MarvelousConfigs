using Marvelous.Contracts.ExchangeModels;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using MassTransit;

namespace MarvelousConfigs.API.RMQ.Producers
{
    public class MarvelousConfigsProducer : IMarvelousConfigsProducer
    {
        private readonly IConfigsService _configService;
        private readonly IMicroservicesService _microservicesService;
        private readonly ILogger<MarvelousConfigsProducer> _logger;
        private readonly IBus _bus;

        public MarvelousConfigsProducer(IConfigsService configsService, IMicroservicesService microservicesService,
            ILogger<MarvelousConfigsProducer> logger, IBus bus)
        {
            _configService = configsService;
            _microservicesService = microservicesService;
            _logger = logger;
            _bus = bus;
        }

        public async Task NotifyConfigurationAdded(int id)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            _logger.LogInformation($"Try publish config");
            ConfigModel config = new ConfigModel() { Id = 1, Key = "TestKey", Value = "TestValue" };
            // var config = await _configService.GetById(id); by key?? 

            await _bus.Publish<ConfigExchangeModel>(new
            {
                config.Id,
                config.Key,
                config.Value
            },
                source.Token);
            _logger.LogInformation($"Config {config.Id} published");
        }
    }
}
