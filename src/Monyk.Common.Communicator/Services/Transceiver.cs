using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Monyk.Common.Communicator.Services
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

    public class TransceiverBase
    {
        protected static readonly Encoding BodyEncoding = Encoding.UTF8;
    }

    public class Transceiver<T> : TransceiverBase, ITransmitter<T>, IReceiver<T>, IDisposable
    {
        public event EventHandler<T> Received;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Transceiver()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void StartReception()
        {
            _channel.QueueDeclare(typeof(T).Name,
                true,
                false,
                false,
                null);

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
            _channel.QueueDeclare(typeof(T).Name,
                true,
                false,
                false,
                null);

            var messageString = Serialize(message);
            var body = BodyEncoding.GetBytes(messageString);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish("",
                typeof(T).Name,
                properties,
                body);
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
