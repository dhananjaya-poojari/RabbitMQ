using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Reciever
{
    /// <summary>
    /// This class provides functionality to receive messages from a RabbitMQ queue
    /// and process them as a background service.
    /// </summary>
    public class RabbitMQQueueReciever : BackgroundService
    {
        private const string queueName = "Sample";
        private const string _hostname = "localhost";
        private const string _username = "guest";
        private const string _password = "guest";
        private IConnection _connection;
        private IModel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQReceiver"/> class.
        /// </summary>
        public RabbitMQQueueReciever()
        {
            // Create a connection factory and establish the connection
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                Password = _password,
                UserName = _username
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the queue to ensure it exists before consuming messages
            _channel.QueueDeclare(queueName, false, false, false, null);
        }

        /// <summary>
        /// This method is called when the <see cref="IHostedService"/> starts.
        /// It begins listening for messages on the specified RabbitMQ queue.
        /// </summary>
        /// <param name="stoppingToken">A cancellation token that can be used to stop the background service.</param>
        /// <returns>A task that represents the background service's execution.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Ensure the task is cancelled if the service is stopped
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // Decode the message content and process it
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var body = JsonConvert.DeserializeObject<string>(content);
                Console.WriteLine(body + "\nRecieved from Queue.\n");

                // Acknowledge the message has been received and processed
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            // Start consuming messages from the queue
            _channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
