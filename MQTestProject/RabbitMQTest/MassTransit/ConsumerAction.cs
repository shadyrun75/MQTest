using MassTransit;
using MQTestProject.Models;

namespace RabbitMQTest.MassTransit
{
    public class ConsumerAction : IConsumer<IWeatherForecast>
    {
        public async Task Consume(ConsumeContext<IWeatherForecast> context)
        {
            Console.WriteLine($" > New data: {context.Message.City} {context.Message.TemperatureC}");
        }
    }
}
