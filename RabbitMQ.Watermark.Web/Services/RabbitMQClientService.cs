using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyRabbitMQWeb.Watermark.Services
{
    /// <summary>
    /// RabbitMQ bağlantısı
    /// </summary>
    public class RabbitMQClientService:IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermark = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";

        private readonly ILogger<RabbitMQClientService> _logger;

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        /// <summary>
        /// Bağlantı kurma
        /// </summary>
        /// <returns></returns>
        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

           //kanal varsa geri dön.
            if(_channel is { IsOpen:true}) //_channel.IsOpen
            {
                return _channel;
            }

            //kanal yok
            _channel = _connection.CreateModel();

            //exchange
            //otomatik silinmesin -false
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false);

            //exchange declare
            //başka channeldan erişcez.2.false
            _channel.QueueDeclare(QueueName, true, false, false, null);

            //kuyruk oluşturma işlemi
            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);

            _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");

            return _channel;

        }

        /// <summary>
        /// Bağlantıları kapatma. 
        /// Kanal kapatma/Dispose
        /// Connection kapatma/Dispose
        /// </summary>
        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
           
            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ ile bağlantı koptu...");
        }
    }
}
