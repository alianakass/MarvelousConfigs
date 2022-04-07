using MarvelousConfigs.API.RMQ.Producers;
using MarvelousConfigs.BLL.Cache;
using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.DAL.Repositories;
using MassTransit;
using NLog.Extensions.Logging;

namespace MarvelousConfigs.API.Extensions
{
    public static class IServiceProviderExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMicroservicesService, MicroservicesService>();
            services.AddScoped<IConfigsService, ConfigsService>();
            services.AddScoped<IMarvelousConfigsProducer, MarvelousConfigsProducer>();
            services.AddTransient<IMemoryCacheExtentions, MemoryCacheExtentions>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMicroserviceRepository, MicroservicesRepository>();
            services.AddScoped<IConfigsRepository, ConfigsRepository>();
        }

        public static void SetMemoryCache(this WebApplication app)
        {
            app.Services.CreateScope().ServiceProvider.GetRequiredService<IMemoryCacheExtentions>().SetMemoryCache();
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

        public static void AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://80.78.240.16", hst =>
                    {
                        hst.Username("nafanya");
                        hst.Password("qwe!23");
                    });
                });
            });
        }
    }
}
