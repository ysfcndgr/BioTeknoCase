using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace BioTekno.Infrastructure.Services.Redis
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisHealthCheck(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_redis.GetDatabase().Ping().TotalMilliseconds > 0)
                {
                    return Task.FromResult(HealthCheckResult.Healthy("Redis is up and running."));
                }
                return Task.FromResult(HealthCheckResult.Unhealthy("Redis is unreachable."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"Redis error: {ex.InnerException.Message ?? ex.Message}"));
            }
        }
    }
}

