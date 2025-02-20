using BioTekno.Application.Dtos;
using BioTekno.Application.Features.Orders.Commands;
using BioTekno.Infrastructure.Services.RabbitMQ;
using BioTekno.Infrastructure.Services.Redis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly RabbitMQHealthCheck _rabbitMQHealthCheck;
    private readonly RedisHealthCheck _redisHealthCheck;


    public HealthCheckController(RabbitMQHealthCheck rabbitMQHealthCheck, RedisHealthCheck redisHealthCheck)
    {
        this._rabbitMQHealthCheck = rabbitMQHealthCheck;
        this._redisHealthCheck = redisHealthCheck;
    }


    [HttpGet("redishealth")]
    public async Task<IActionResult> RedisHealth()
    {
        var context = new HealthCheckContext();
        var report = await _redisHealthCheck.CheckHealthAsync(context);

        return report.Status == HealthStatus.Healthy
            ? Ok(report)
            : StatusCode(500, report);
    }

    [HttpGet("rabbitmqhealth")]
    public async Task<IActionResult> RabbitMQHealth()
    {
        var context = new HealthCheckContext();
        var report = await _rabbitMQHealthCheck.CheckHealthAsync(context);

        return report.Status == HealthStatus.Healthy
            ? Ok(report)
            : StatusCode(500, report);
    }
}