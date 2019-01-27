namespace Monyk.Common.Communicator
{
    public class CommunicatorSettings
    {
        public class RabbitMQSettings
        {
            public string Host { get; set; }
            public string VHost { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public RabbitMQSettings RabbitMQ { get; set; }
    }
}
