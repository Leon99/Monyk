using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Monyk.Common.Communicator
{
    public interface ITransmitter<in T>
    {
        void Transmit(T message);
    }

    public interface IReceiver<T>
    {
        event EventHandler<T> Received;
        void StartReception();
    }

    public abstract class TransceiverBase
    {
        protected static readonly Encoding BodyEncoding = Encoding.UTF8;
    }

    public class Transceiver<T> : TransceiverBase, ITransmitter<T>, IReceiver<T>, IDisposable
    {
        public event EventHandler<T> Received;

        private readonly ILogger<Transceiver<T>> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Transceiver(ILogger<Transceiver<T>> logger, IConnectionFactory factory)
        {
            _logger = logger;
            var retryDelay = TimeSpan.FromSeconds(10);
            var retryCount = 3;
            IConnection connection = null;
            Policy
                .Handle<BrokerUnreachableException>()
                .Or<ConnectFailureException>()
                .WaitAndRetry(
                    retryCount, 
                    i => retryDelay,
                    (ex, delay, i, ctx) => 
                        _logger.LogWarning($"Unable to connect to RabbitMQ. Will retry in {delay.TotalSeconds} seconds {retryCount - i} more time(s)."))
                .Execute(() => 
                    connection = factory.CreateConnection());
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public void StartReception()
        {
            DeclareQueue();

            _channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var messageString = BodyEncoding.GetString(body);
                var message = Deserialize(messageString);
                OnReceived(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(typeof(T).Name,
                false,
                consumer);
        }

        private string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private T Deserialize(string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

        public void Transmit(T message)
        {
            DeclareQueue();

            var messageString = Serialize(message);
            var body = BodyEncoding.GetBytes(messageString);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish("",
                typeof(T).Name,
                properties,
                body);
        }

        private void DeclareQueue()
        {
            _channel.QueueDeclare(
                typeof(T).Name,
                true,
                false,
                true,
                null);
        }

        protected virtual void OnReceived(T message)
        {
            Received?.Invoke(this, message);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
