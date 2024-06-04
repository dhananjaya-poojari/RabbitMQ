namespace Sender
{
    public interface IRabbitMQSender
    {
        void Send(string message);
    }
}
