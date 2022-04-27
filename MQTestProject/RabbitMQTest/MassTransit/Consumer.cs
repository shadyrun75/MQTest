using MassTransit;
using MQTestProject.Models;
using RabbitMQ.Client;
using RabbitMQTest;

namespace RabbitMQTest.MassTransit
{
    public class Consumer
    {
        public async Task Main(string host, ushort port, string exchange = "weather-exchange")
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

            //var services = new ServiceCollection()
            //    .AddMassTransit(x =>
            //    {
            //        x.UsingRabbitMq((context, cfg) =>
            //        {
            //            cfg.Host("localhost", "/", h =>
            //            {
            //                h.Username("guest");
            //                h.Password("guest");
            //            });

            //            cfg.ReceiveEndpoint("weather-exchange", e =>
            //            {
            //                e.Consumer<Consumer>();
            //                e.ConfigureConsumer<Consumer>(context);
            //                e.Bind<IWeatherForecast>(x =>
            //                {
            //                    x.RoutingKey = rk;
            //                    x.ExchangeType = ExchangeType.Topic;
            //                });
            //            });

            //            cfg.AutoDelete = false;
            //            cfg.AutoStart = true;
            //            cfg.Durable = true;
            //        });
            //    })
            //    .BuildServiceProvider();
            //while (Console.ReadKey().Key != ConsoleKey.Escape)
            //{
            //    Console.WriteLine(" > Press Escape for exit");
            //}

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(host, port, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                
                cfg.ReceiveEndpoint(queue, e =>
                {
                    e.AutoDelete = true;
                    e.AutoStart = true;
                    e.BindQueue = true;

                    //If you are binding the messages types to the receive endpoint that are the same as message types in the consumer,
                    //you need to disable the automatic exchange binding.
                    e.ConfigureConsumeTopology = false;
                    e.Consumer<ConsumerAction>();
                    //e.BindDeadLetterQueue("weather-exchange", queueName, x =>  // создается куча очередей + к нужной очереди не цепляется
                    e.Bind(exchange, x =>
                    //e.Bind<IWeatherForecast>(x =>
                    {
                        x.RoutingKey = rk;
                        x.ExchangeType = ExchangeType.Topic;
                        x.Durable = true;
                        x.AutoDelete = false;
                    });
                });

                cfg.AutoDelete = true;
                cfg.AutoStart = true;
                cfg.Durable = true;
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        
    }
}
