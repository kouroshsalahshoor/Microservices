namespace MessageSenders
{
    public interface ISendMessage
    {
        void Send(string message, string queueName);
    }
}
