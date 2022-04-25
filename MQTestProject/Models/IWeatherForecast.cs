namespace MQTestProject.Models
{
    public interface IWeatherForecast
    {
        string City { get; set; }
        DateTime Date { get; set; }
        int TemperatureC { get; set; }
        int TemperatureF { get; }
    }
}