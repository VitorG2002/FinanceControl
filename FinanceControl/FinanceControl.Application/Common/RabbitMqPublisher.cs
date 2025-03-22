using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using NotificationService;

namespace FinanceControl.FinanceControl.Application.Common
{
    public class RabbitMqPublisher
    {
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMqPublisher()
        {
        }

        public async Task PublishNotification(NotificationMessage notification)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "user",  // Substitua pelo seu nome de usuário
                Password = "MyNewPassword123"   // Substitua pela sua senha
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync(
                queue: "notifications",
                durable: true,
                exclusive: false,
                autoDelete: false);

            var message = JsonConvert.SerializeObject(notification);
            var body = Encoding.UTF8.GetBytes(message);

            // Corrigindo a chamada do BasicPublishAsync
            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: "notifications",
                body: new ReadOnlyMemory<byte>(body));

            if (_channel != null)
                await _channel.CloseAsync();
            if (_connection != null)
                await _connection.CloseAsync();
        }
    }
}