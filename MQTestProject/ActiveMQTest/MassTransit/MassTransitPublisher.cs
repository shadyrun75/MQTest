using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MQTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQTest
{
    public class MassTransitPublisher
    {
        
        public async static Task Main(string host, int port)
        {
            Console.Clear();
            Console.WriteLine("Enter city name: ");
            var city = Console.ReadLine() ?? "";

            var services = new ServiceCollection()
                .AddMassTransit(x =>
                {
                    x.UsingActiveMq((context, cfg) =>
                    {
                        cfg.Host(host, port, h =>
                        {
                            h.Username("admin");
                            h.Password("admin");
                        });

                        cfg.Message<IWeatherForecast>(c =>
                        {
                            c.SetEntityName("weather-exchange");
                        });


                        cfg.AutoDelete = true;
                        cfg.AutoStart = true;
                        cfg.Durable = true;
                    });
                })
                .AddSingleton<IPublisherByPublic, PublisherByPublic>()
                .BuildServiceProvider();

            var publisher = services.GetService<IPublisherByPublic>();
            publisher.City = city;
            await publisher.Start();
        }
    }

    /// <summary>
    /// Класс публикации сообщения. Publish - это отправка сообщения в очередь, которое забирает любой из Consumer, который подписан на определенные очереди.
    /// Тут используются СОБЫТИЯ, а не КОМАНДЫ.
    /// </summary>
    public class PublisherByPublic : IPublisherByPublic
    {
        readonly IPublishEndpoint _publishEndpoint;
        public PublisherByPublic(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public string City { get; set; } = "";

        public async Task Start()
        {
            while (true)
            {
                var value = ReadData();
                if (value == null)
                {
                    Console.WriteLine("Error input data. Publisher stoped.");
                    break;
                }
                await Publish(value);
            }
            await Task.CompletedTask;
        }

        private WeatherForecast ReadData()
        {
            Console.WriteLine("Enter temperature");
            var value = Console.ReadLine();
            if (Int32.TryParse(value, out var tempInt))
                return new WeatherForecast()
                {
                    City = this.City,
                    Date = DateTime.Now,
                    TemperatureC = tempInt
                };
            else
                return null;
        }

        private async Task Publish(IWeatherForecast value)
        {
            await _publishEndpoint.Publish<IWeatherForecast>(value);
        }
    }

    public interface IPublisherByPublic
    {
        string City { get; set; }

        Task Start();
    }
}
