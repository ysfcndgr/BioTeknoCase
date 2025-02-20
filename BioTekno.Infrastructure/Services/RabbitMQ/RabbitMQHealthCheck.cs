using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace BioTekno.Infrastructure.Services.RabbitMQ
{
    public class RabbitMQHealthCheck : IHealthCheck
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMQHealthCheck()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = Constants.rabbitMQhostName,
                UserName = Constants.rabbitMQUserName,
                Password = Constants.rabbitMQPassword,
                VirtualHost = Constants.rabbitMQVirtualHost,
                Port = 5671,
                Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = Constants.rabbitMQServerName
                }
            };
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();

                return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ is reachable."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"RabbitMQ error: {ex.Message}"));
            }
        }
    }

}