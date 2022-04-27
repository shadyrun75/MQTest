using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTestProject.Models
{
    public class WeatherReader
    {
        public WeatherReader()
        {
            Console.WriteLine("Enter city");
            City = Console.ReadLine() ?? "";
        }

        public delegate void OnPublishHandler(IWeatherForecast value);
        public event OnPublishHandler OnPublish;

        public string City { get; private set; } = "";

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
                OnPublish?.Invoke(value);
            }
            await Task.CompletedTask;
        }

        private IWeatherForecast ReadData()
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

    }
}