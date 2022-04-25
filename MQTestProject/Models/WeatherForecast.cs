namespace MQTestProject.Models
{
    public class WeatherForecast : IWeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string City { get; set; }

        public static WeatherForecast Get(string city)
        {
            Console.WriteLine("Enter temperature\r\n > ");
            if (!Int32.TryParse(Console.ReadLine(), out var temp))
            {
                throw new ArgumentException("Temperature error");
            }
            return new WeatherForecast()
            {
                City = city,
                Date = DateTime.Now,
                TemperatureC = temp
            };
        }
    }
}