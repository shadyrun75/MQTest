using MassTransit;
using MQTestProject.Models;

namespace RabbitMQTest.MassTransit
{
    public class PublisherByPublic : IPublisherByPublic
    {
        readonly IPublishEndpoint _publishEndpoint;
        public PublisherByPublic(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Start()
        {
            WeatherReader weatherReader = new WeatherReader();
            weatherReader.OnPublish += WeatherReader_OnPublish;
            await weatherReader.Start();
        }

        private void WeatherReader_OnPublish(IWeatherForecast value)
        {
            _publishEndpoint.Publish<IWeatherForecast>(value);
        }
    }
}
