using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMqListener(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Inicialização da conexão e canal de forma assíncrona
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "user",  // Substitua pelo seu nome de usuário
                Password = "MyNewPassword123"   // Substitua pela sua senha
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Declarando a fila de forma assíncrona
            await _channel.QueueDeclareAsync(
                queue: "notifications",
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Cria o consumidor assíncrono
            var consumer = new AsyncEventingBasicConsumer(_channel);

            // O evento agora se chama ReceivedAsync e aceita somente o argumento (ea)
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationEmailService>();
                    await notificationService.ProcessMessageAsync(message);
                }

                // Reconhece a mensagem de forma assíncrona
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            // Inicia o consumo de mensagens de forma assíncrona
            await _channel.BasicConsumeAsync(
                queue: "notifications",
                autoAck: false,
                consumer: consumer);

            // Mantém o serviço ativo até que o token de cancelamento seja disparado
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
                await _channel.CloseAsync();
            if (_connection != null)
                await _connection.CloseAsync();

            await base.StopAsync(cancellationToken);
        }
    }
}