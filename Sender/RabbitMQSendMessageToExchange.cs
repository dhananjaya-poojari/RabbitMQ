using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Sender
{
    /// <summary>
    /// This class provides functionality to send messages to a RabbitMQ exchange.
    /// </summary>
    public class RabbitMQSendMessageToExchange : IRabbitMQSender
    {
        /// <summary>
        /// The name of the exchange to send messages to.
        /// </summary>
        private const string exchangeName = "SampleExchange";

        /// <summary>
        /// The hostname of the RabbitMQ server.
        /// </summary>
        private const string _hostname = "localhost";

        /// <summary>
        /// The username to connect to the RabbitMQ server.
        /// </summary>
        private const string _username = "guest";

        /// <summary>
        /// The password to connect to the RabbitMQ server.
        /// </summary>
        private const string _password = "guest";

        /// <summary>
        /// The connection to the RabbitMQ server.
        /// </summary>
        private IConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQSendMessageToExchange"/> class.
        /// </summary>
        public RabbitMQSendMessageToExchange()
        {
        }

        /// <summary>
        /// Sends a message to the RabbitMQ exchange.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void Send(string message)
        {
            // Ensure a connection to the RabbitMQ server is established
            CreateConnection();

            // Create a new channel, which is the medium for sending messages
            using var channel = _connection.CreateModel();

            // Declare an exchange with the specified name and type. If it doesn't exist, it will be created.
            // Parameters:
            // exchange: The name of the exchange.
            // type: The type of the exchange (e.g., fanout).
            // durable: Whether the exchange should survive a broker restart (false means it won't).
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: false);

            // Serialize the message to a JSON string.
            var json = JsonConvert.SerializeObject(message);

            // Convert the JSON string to a byte array, which is the format required by RabbitMQ.
            var body = Encoding.UTF8.GetBytes(json);

            // Publish the message to the specified exchange.
            // Parameters:
            // exchange: The exchange to publish the message to.
            // routingKey: The routing key for the message (empty string in this case because fanout exchanges do not use routing keys).
            // basicProperties: Message properties (null in this case, meaning default properties).
            // body: The message body as a byte array.
            channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
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
