using System;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class WeatherService
    {
        private static readonly Random Rand = new Random();
        private static readonly string[] Conditions = {"Cloudy", "Sunshine", "Overcast", "Rain"};

        public async Task<WeatherModel> GetWeatherAsync()
        {
            // Simulate long-running data retrieving.
            var temperature = await Task.FromResult(Rand.Next(-20, 50));
            await Task.Delay(new TimeSpan(0, 0, 5));

            var wind = await Task.FromResult(Rand.Next(0, 10));
            await Task.Delay(new TimeSpan(0, 0, 5));

            var condition = await Task.FromResult(Conditions[Rand.Next(0, Conditions.Length - 1)]);
            await Task.Delay(new TimeSpan(0, 0, 5));

            return new WeatherModel {Temperature = temperature, Wind = wind, Condition = condition};
        }
    }
}