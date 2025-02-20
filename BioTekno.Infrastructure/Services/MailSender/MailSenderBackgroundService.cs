using BioTekno.Domain.Interfaces;
using BioTekno.Infrastructure.Services.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.Ocsp;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BioTekno.Infrastructure.Services
{
    public sealed class MailSenderBackgroundService : BackgroundService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILoggerService<MailSenderBackgroundService> _logger;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IMailService _mailService;

        public MailSenderBackgroundService(ILoggerService<MailSenderBackgroundService> logger, IRabbitMqService rabbitMqService, IMailService mailService)
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
            _logger = logger;
            _rabbitMqService = rabbitMqService;
            _mailService = mailService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "SendMail",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var mailObject = System.Text.Json.JsonSerializer.Deserialize<MailMessageDto>(message);

                try
                {
                  await _mailService.SendEmailAsync(mailObject.To, mailObject.Subject, mailObject.Body, true);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Mail gönderilirken hata oluştu: {ex.Message}");

                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }

                await Task.CompletedTask;
            };

            _channel.BasicConsume(queue: "SendMail",
                                  autoAck: false, 
                                  consumer: consumer);

            return Task.CompletedTask;
        }


        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

    public class MailMessageDto
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
