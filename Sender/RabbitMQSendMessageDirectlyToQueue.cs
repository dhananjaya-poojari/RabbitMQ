using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Sender
{
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using System;
    using System.Text;

    namespace Messaging
    {
        /// <summary>
        /// This class provides functionality to send messages directly to a RabbitMQ queue.
        /// </summary>
        public class RabbitMQSendMessageDirectlyToQueue : IRabbitMQSender
        {
            /// <summary>
            /// The name of the queue to send messages to.
            /// </summary>
            private const string queueName = "Sample";

            /// <summary>
            /// The hostname of the RabbitMQ server.
            /// </summary>
            private readonly string _hostname;

            /// <summary>
            /// The username to connect to the RabbitMQ server.
            /// </summary>
            private readonly string _username;

            /// <summary>
            /// The password to connect to the RabbitMQ server.
            /// </summary>
            private readonly string _password;

            /// <summary>
            /// The connection to the RabbitMQ server.
            /// </summary>
            private IConnection _connection;

            /// <summary>
            /// Initializes a new instance of the <see cref="RabbitMQSendMessageDirectlyToQueue"/> class.
            /// </summary>
            public RabbitMQSendMessageDirectlyToQueue()
            {
                _hostname = "localhost";
                _username = "guest";
                _password = "guest";
            }

            /// <summary>
            /// Sends a message to the RabbitMQ queue.
            /// </summary>
            /// <param name="message">The message to send.</param>
            public void Send(string message)
            {
                // Ensure a connection to the RabbitMQ server is established
                CreateConnection();

                // Create a new channel, which is the medium for sending messages
                using var channel = _connection.CreateModel();

                // Declare a queue with the specified name. If it doesn't exist, it will be created.
                // Parameters:
                // queue: The name of the queue.
                // durable: Whether the queue should survive a broker restart (false means it won't).
                // exclusive: Whether the queue is used by only one connection and will be deleted when that connection closes.
                // autoDelete: Whether the queue should be deleted when there are no more consumers.
                // arguments: Additional arguments for the queue (null in this case).
                channel.QueueDeclare(queueName, false, false, false, null);

                // Serialize the message to a JSON string.
                var json = JsonConvert.SerializeObject(message);

                // Convert the JSON string to a byte array, which is the format required by RabbitMQ.
                var body = Encoding.UTF8.GetBytes(json);

                // Publish the message to the specified queue.
                // Parameters:
                // exchange: The exchange to publish the message to (empty string means the default exchange).
                // routingKey: The routing key for the message (the name of the queue in this case).
                // basicProperties: Message properties (null in this case, meaning default properties).
                // body: The message body as a byte array.
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }

            /// <summary>
            /// Creates a connection to the RabbitMQ server if one does not already exist.
            /// </summary>
            private void CreateConnection()
            {
                // Check if the _connection is null, indicating that there is no active connection to RabbitMQ
                if (_connection == null)
                {
                    try
                    {
                        // Create a new instance of the ConnectionFactory class, which configures the connection settings
                        var factory = new ConnectionFactory()
                        {
                            // Set the hostname of the RabbitMQ server
                            HostName = _hostname,
                            // Set the password for the RabbitMQ server
                            Password = _password,
                            // Set the username for the RabbitMQ server
                            UserName = _username
                        };

                        // Create a new connection to the RabbitMQ server using the settings from the factory
                        _connection = factory.CreateConnection();
                    }
                    catch (Exception ex)
                    {
                        // If an exception occurs while creating the connection, write the exception details to the console
                        Console.WriteLine(ex.ToString());
                    }
                }

            }
        }
    }

}
