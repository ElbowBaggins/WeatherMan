using System;
using System.Threading.Tasks;
using Hemogoblin.YWeather;

namespace Hemogoblin.WeatherMan
{
    public class WeatherMan
    {
        private readonly string location;
        public WeatherData WeatherData { get; private set; }

        public WeatherMan(string location)
        {
            this.location = location;
        }

        public async Task<WeatherData> GetYahooWeather(string locationToGet)
        {
            return await WeatherData.GetWeatherAsync(location);
        }

        public async Task Start()
        {
            Console.WriteLine("Querying Yahoo! Weather. This may take a moment...");
            WeatherData = await GetYahooWeather(location);
            Console.Write("\r\n" +
                              "Weather data for: " + WeatherData.Location + "\r\n" +
                              "Last updated: " + WeatherData.TimeUpdated + "\r\n\r\n" +
                              "Conditions: " + WeatherData.Conditions + ".\r\n" +
                              "Temperature: " + WeatherData.Temperature);
            if(!WeatherData.Temperature.Equals(WeatherData.WindChill))
            {
                Console.Write(" (" + WeatherData.WindChill + " including wind chill.)");
            }
            Console.Write("\r\nHumidity: " + WeatherData.Humidity + "\r\n" +
                          "Visibility: " + WeatherData.Visibility + ".\r\n" +
                          "Barometric Pressure: " + WeatherData.Pressure + ".\r\n" +
                          "Wind: " + WeatherData.WindSpeed);
            if(!WeatherData.WindSpeed.Equals("Still"))
            {
                Console.Write(", " + WeatherData.WindDirection);
            }
            Console.Write(".\r\n\r\n" +
                          "Sunrise: " + WeatherData.Sunrise + "\r\n" +
                          "Sunset: " + WeatherData.Sunset + "\r\n");
        }
    }
}
