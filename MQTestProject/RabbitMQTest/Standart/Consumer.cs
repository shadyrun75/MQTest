using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTestProject.Models;

namespace RabbitMQTest.Standart
{
    public class Consumer
    {
        public async Task Main(string host, int port, string exchange = "weather-exchange")
        {
            Console.Clear();
            var settings = new ConsumerSettings();
            settings.Init();
            var rk = RoutingKeyGenerator.Get(settings);
            var queue = $"{exchange}:";
            if (rk == "#")
                queue += "all";
            else
                queue += $"{settings.City}_{settings.Temperature}";

            var factory = new ConnectionFactory();
            factory.HostName = host;
            factory.Port = port;
            factory.VirtualHost = "/";
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.RequestedHeartbeat = new TimeSpan(0, 0, 1);
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // объявление обменника
                    channel.ExchangeDeclare(
                        exchange: exchange,
                        type: "topic",
                        durable: true,
                        autoDelete: false);

                    // объявление очереди
                    channel.QueueDeclare(
                        queue: queue,
                        durable: false, // сохранение очереди, т.е. при перезапуске сервера очередь восстанавливается
                        exclusive: false, // для каждого соединения своя очередь
                        autoDelete: false, // автоматическое удаление очереди после отключения всех соединений
                        arguments: null); // время жизни сообщения, ограничение очереди
                    
                    // сопряжение очереди с обменником по ключу
                    channel.QueueBind(
                        queue: queue, 
                        exchange: exchange, 
                        routingKey: rk, 
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: queue,
                                         autoAck: true, // возврат сообщения в очередь
                                         consumer: consumer);

                    await ConsumerWaiter.Run();
                }
            }
        }
    }
}
