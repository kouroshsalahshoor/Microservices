namespace Shared.RabbitMQSender
{
    public interface IRabbitMQSender
    {
        void Send(string message, string queueName);
    }
}
