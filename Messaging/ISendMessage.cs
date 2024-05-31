namespace MessageSenders
{
    public interface ISendMessage
    {
        void Send(object message, string queueName);
        void Send(string message, string queueName);
    }
}
