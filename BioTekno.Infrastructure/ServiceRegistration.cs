using System;
using BioTekno.Domain.Interfaces;
using BioTekno.Infrastructure.Services;
using BioTekno.Infrastructure.Services.Logger;
using BioTekno.Infrastructure.Services.MailSender;
using BioTekno.Infrastructure.Services.RabbitMQ;
using BioTekno.Infrastructure.Services.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;

namespace BioTekno.Infrastructure
{
	public static class ServiceRegistration
	{
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddHostedService<MailSenderBackgroundService>();
            services.AddHealthChecks()
                .AddCheck<RedisHealthCheck>("Redis")
                .AddCheck<RabbitMQHealthCheck>("RabbitMQ");

            services.AddSingleton<RedisHealthCheck>();
            services.AddSingleton<RabbitMQHealthCheck>();
            var redisConnectionString = configuration.GetSection("Redis:ConnectionString").Value;
            services.AddSingleton<IConnectionMultiplexer>(x =>
            ConnectionMultiplexer.Connect(redisConnectionString));

            var logger = new LoggerConfiguration()
                   .WriteTo.Console()
                   .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                   .Enrich.FromLogContext()
                   .CreateLogger();

            services.AddSingleton<ILogger>(logger); 

            services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));

            services.AddSingleton<IMailService>(new MailService(
             Constants.yandexMailHost,
             Constants.yandexMailPort,
             Constants.yandexMailUsername,
             Constants.yandexMailPassword
         ));
        }
    }
}

