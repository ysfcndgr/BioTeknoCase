using System;
namespace BioTekno.Domain.Interfaces
{
    public interface ILoggerService<T> where T : class
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex = null);
    }
}

