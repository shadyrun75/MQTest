using MQTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest
{
    public class RoutingKeyGenerator
    {
        public static string Get(string city, string temperature)
        {
            if ((city == String.Empty) && (temperature == String.Empty))
                return "#";
            else
            {
                var cityRK = (city == String.Empty ? "*" : city);
                var temperatureRK = (temperature == String.Empty ? "*" : temperature);
                return $"{temperatureRK}.{cityRK}";
            };
        }

        public static string Get(ConsumerSettings value)
        {
            return Get(value.City, value.Temperature);
        }

        public static string Get(IWeatherForecast value)
        {
            return Get(value.City, value.TypeTemperature);
        }
    }
}
