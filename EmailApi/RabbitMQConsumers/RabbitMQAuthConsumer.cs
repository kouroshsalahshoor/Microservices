
using EmailApi.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace EmailApi.RabbitMQConsumers
{
    public class RabbitMQAuthConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailLoggerService _emailLoggerService;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public RabbitMQAuthConsumer(IConfiguration configuration, EmailLoggerService emailLoggerService)
        {
            _configuration = configuration;
            _emailLoggerService = emailLoggerService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _queueName = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue")!;
            _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, null);

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ev) =>
            {
                var content = Encoding.UTF8.GetString(ev.Body.ToArray());
                var email = JsonConvert.DeserializeObject<string>(content);
                await handleMessage(email!);

                _channel.BasicAck(ev.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task handleMessage(string email)
        {
            await _emailLoggerService.RegisterUser(email);
        }
    }
}
