using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.DAL.Repositories;
using NLog.Extensions.Logging;

namespace MarvelousConfigs.API.Extensions
{
    public static class IServiceProviderExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMicroservicesService, MicroservicesService>();
            services.AddScoped<IConfigsService, ConfigsService>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMicroserviceRepository, MicroservicesRepository>();
            services.AddScoped<IConfigsRepository, ConfigsRepository>();
        }

        public static void RegisterLogger(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
            service.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog(config);
            });
        }
    }
}
