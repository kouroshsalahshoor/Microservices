
using EmailApi.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Dtos.Cart;
using System.Text;

namespace EmailApi.MessageConsumers
{
    public class RabbitMQCartConsumer : BackgroundService
    {
        private readonly EmailLoggerService _emailLoggerService;
        private IModel _channel;
        private string _queueName;

        public RabbitMQCartConsumer(IConfiguration configuration, EmailLoggerService emailLoggerService)
        {
            _emailLoggerService = emailLoggerService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _queueName = configuration.GetValue<string>("TopicAndQueueNames:CartQueue")!;
            _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, null);

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ev) =>
            {
                var content = Encoding.UTF8.GetString(ev.Body.ToArray());
                var cart = JsonConvert.DeserializeObject<CartDto>(content);
                await handleMessage(cart!);

                _channel.BasicAck(ev.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task handleMessage(CartDto dto)
        {
            await _emailLoggerService.Cart(dto);
        }
    }
}
