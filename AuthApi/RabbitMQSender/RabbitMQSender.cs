using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.RabbitMQSender;
using System.Text;

namespace AuthApi.RabbitMQSender
{
    public class RabbitMQSender : IRabbitMQSender
    {
        private readonly string _hostname;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMQSender()
        {
            _hostname = "localhost";
            _userName = "guest";
            _password = "guest";
        }
        public void Send(string message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _userName,
                Password = _password
            };

            _connection = factory.CreateConnection();
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queueName);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
        }
    }
}
