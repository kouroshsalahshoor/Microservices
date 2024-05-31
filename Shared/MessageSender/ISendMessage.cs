namespace Shared.MessageSender
{
    public interface ISendMessage
    {
        void Send(string message, string queueName);
    }
}
