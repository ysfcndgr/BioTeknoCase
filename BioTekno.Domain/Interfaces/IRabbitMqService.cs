using System;
namespace BioTekno.Domain.Interfaces
{
    public interface IRabbitMqService
    {
        void PublishMailQueue(string toEmail, string subject, string body, bool isHtml = false);
    }
}

