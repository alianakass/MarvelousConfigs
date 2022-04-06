using MassTransit;

namespace MarvelousConfigs.API.WorkService
{
    public class ConfigsWorkService : BackgroundService
    {
        private readonly ILogger<ConfigsWorkService> _logger;
        private readonly IBus _bus;

        public ConfigsWorkService(IBus bus, ILogger<ConfigsWorkService> logger)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ConfigsWorkService is starting.");

            await ExecuteAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug($"");


                await Task.Delay(300000, cancellationToken);
            }

            _logger.LogDebug($"");
            await StopAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ConfigsWorkService is stopping.");
        }

    }
}
