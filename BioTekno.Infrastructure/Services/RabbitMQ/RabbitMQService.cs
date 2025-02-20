using System;
using RabbitMQ.Client;
using System.Text;
using BioTekno.Domain.Interfaces;

namespace BioTekno.Infrastructure.Services.RabbitMQ
{

    public sealed class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqService()
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
                },
                AutomaticRecoveryEnabled = true
            };
        }

        public void PublishMailQueue(string toEmail, string subject, string body, bool isHtml = false)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "SendMail",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var mailMessage = new { To = toEmail, Subject = subject, Body = body };
            var messageBody = System.Text.Json.JsonSerializer.Serialize(mailMessage);
            var messageBytes = Encoding.UTF8.GetBytes(messageBody);

            channel.BasicPublish(exchange: "",
                                 routingKey: "SendMail",
                                 basicProperties: null,
                                 body: messageBytes);
        }
    }

}

