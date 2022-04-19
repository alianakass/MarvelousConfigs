﻿using FluentValidation.AspNetCore;
using MarvelousConfigs.API.Models.Validation;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.DAL.Repositories;
using MassTransit;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;

namespace MarvelousConfigs.API.Extensions
{
    public static class IServiceProviderExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMicroservicesService, MicroservicesService>();
            services.AddScoped<IConfigsService, ConfigsService>();
            services.AddScoped<IAuthRequestClient, AuthRequestClient>();
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
            app.Services.CreateScope().ServiceProvider
                .GetRequiredService<IMemoryCacheExtentions>().SetMemoryCache();
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
                x.UsingRabbitMq((context, cfg) => { });
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.EnableAnnotations();
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyAPI",
                    Version = "v1"
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
                });
            });
            services.AddFluentValidationRulesToSwagger();
        }

        public static void AddFluentValidation(this IServiceCollection services)
        {
            //Добавление FluentValidation
            services.AddFluentValidation(fv =>
            {
                //Регистрация валидаторов по сборке с временем жизни = Singleton
                fv.RegisterValidatorsFromAssemblyContaining<AuthRequestModelValidator>(lifetime: ServiceLifetime.Singleton);
                fv.RegisterValidatorsFromAssemblyContaining<ConfigInputModelValidator>(lifetime: ServiceLifetime.Singleton);
                //Отключение валидации с помощью DataAnnotations
                fv.DisableDataAnnotationsValidation = true;
            });
            //Отключение стандартного валидатора
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        }
    }
}
