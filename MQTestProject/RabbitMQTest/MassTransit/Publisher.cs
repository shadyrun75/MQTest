using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MQTestProject.Models;
using RabbitMQ.Client;
using RabbitMQTest;

namespace RabbitMQTest.MassTransit
{
    public class Publisher
    {
        public async Task Main(string host, int port, string exchange = "weather-exchange")
        {
            var services = new ServiceCollection()
                .AddMassTransit(x =>
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            { 
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.Message<IWeatherForecast>(c => c.SetEntityName(exchange));
                            cfg.Publish<IWeatherForecast>(c => c.ExchangeType = ExchangeType.Topic); 
                            // ExchangeType:
                            // direct работает только по routingkey - одному слову
                            // fanout работает со всеми подписчиками игнорируя routingkey
                            // topic работает c подписчиками по bind и routingkey может быть составной *.word.# где * - одно слово, # - 0 или много слов
                            // headers работает только с аттрибутами и значениями, игнорируя routingkey
                            cfg.Send<IWeatherForecast>(c => c.UseRoutingKeyFormatter(cnt =>
                            {
                                return RoutingKeyGenerator.Get(cnt.Message);
                            })); 
                            cfg.AutoDelete = true;
                            cfg.AutoStart = true;
                            cfg.Durable = true;                            
                        });
                    })
                .AddSingleton<IPublisherByPublic, PublisherByPublic>()
                .BuildServiceProvider();

            var publisher = services.GetService<IPublisherByPublic>();
            await publisher.Start();            
        }

    }
}
