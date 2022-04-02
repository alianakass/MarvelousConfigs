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
            services.AddSingleton<IConfigCache, ConfigCache>();
            services.AddSingleton<IMicroserviceCache, MicroserviceCache>();
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

        public static void AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                //x.AddConsumer<>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://80.78.240.16", hst =>
                    {
                        hst.Username("nafanya");
                        hst.Password("qwe!23");
                    });

                    //cfg.Host("rabbitmq://localhost", hst =>
                    //{
                    //    hst.Username("guest");
                    //    hst.Password("guest");
                    //});

                    //cfg.ReceiveEndpoint("", x =>
                    //{
                    //    x.ConfigureConsumer<>(context);
                    //});

                });
            });
        }
    }
}
