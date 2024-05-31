using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MessageSenders
{
    public class RabbitMQSender : ISendMessage
    {
        private readonly string _hostname;
        private readonly string _userName;
        private readonly string _password;
        private IConnection? _connection;

        public RabbitMQSender()
        {
            _hostname = "localhost";
            _userName = "guest";
            _password = "guest";
        }
        public void Send(object message, string queueName)
        {
            if (connectionExists())
            {
                using var channel = _connection!.CreateModel();
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }

        }
        public void Send(string message, string queueName)
        {
            if (connectionExists())
            {
                using var channel = _connection!.CreateModel();
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }

        }

        private void createConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _userName,
                    Password = _password
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
            }
        }

        private bool connectionExists()
        {
            if (_connection is null)
            {
                createConnection();
                return true;
            }
            return false;
        }
    }
}
