using System;
using BioTekno.Domain.Interfaces;
using Serilog;

namespace BioTekno.Infrastructure.Services.Logger
{

    public class LoggerService<T> : ILoggerService<T> where T : class
    {
        private readonly ILogger _logger;

        public LoggerService(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.Information("[{Source}] {Message}", typeof(T).Name, message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.Error(ex, "[{Source}] {Message}", typeof(T).Name, message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning("[{Source}] {Message}", typeof(T).Name, message);
        }
    }

}