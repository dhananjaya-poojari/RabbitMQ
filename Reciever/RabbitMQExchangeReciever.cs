using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Reciever
{
    public class RabbitMQExchangeReciever
    {
        private const string exchangeName = "SampleExchange";
        private const string _hostname = "localhost";
        private const string _username = "guest";
        private const string _password = "guest";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string queuename;

        public RabbitMQExchangeReciever()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                Password = _password,
                UserName = _username
            };

            // Create a new connection to RabbitMQ using the configured connection factory
            _connection = factory.CreateConnection();

            // Create a new channel, which acts as a communication pathway to RabbitMQ
            _channel = _connection.CreateModel();

            // Declare an exchange on the channel with the specified name and type
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, false);

            // Declare a new queue on the channel and retrieve its name
            queuename = _channel.QueueDeclare().QueueName;

            // Bind the newly created queue to the declared exchange with an empty routing key
            // This means the queue will receive all messages published to the exchange
            _channel.QueueBind(queuename, exchangeName, "");
        }
        public async Task Start()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var obj = JsonConvert.DeserializeObject<string>(content);

                Console.WriteLine(obj + "\nRecieved from Exchange.\n");

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queuename, false, consumer);
        }

        public async Task Stop()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
