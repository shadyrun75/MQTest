using MQTestProject.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Standart
{
    public class Publisher : IDisposable
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        string _exchange;
        public async Task Main(string host, int port, string exchange = "weather-exchange")
        {
            _exchange = exchange;
            factory = new ConnectionFactory();
            factory.HostName = host;
            factory.Port = port;
            factory.VirtualHost = "/";
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.RequestedHeartbeat = new TimeSpan(0, 0, 1);

            WeatherReader weatherReader = new WeatherReader();
            weatherReader.OnPublish += WeatherReader_OnPublish;
            await weatherReader.Start();
        }

        private void WeatherReader_OnPublish(IWeatherForecast value)
        {
            if (connection == null)
                connection = factory.CreateConnection();
            if (channel == null)
            {
                channel = connection.CreateModel();
                // объявление обменника, если его вдруг нет
                channel.ExchangeDeclare(
                    exchange: _exchange,
                    type: "topic",
                    durable: true,
                    autoDelete: false);
            }

            var body = ObjectHelper.ObjectToJsonToByteArray(value);
            channel.BasicPublish(
                exchange: _exchange,
                routingKey: RoutingKeyGenerator.Get(value),
                basicProperties: null,
                body: body);
            System.Threading.Thread.Sleep(1);
            Console.WriteLine(" sended ");
        }

        public void Dispose()
        {
            channel?.Dispose();
            connection?.Dispose();
        }
    }
}
