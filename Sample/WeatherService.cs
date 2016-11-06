using System;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class WeatherService
    {
        private static readonly Random _rand = new Random();
        private static readonly string[] _conditions = { "Cloudy", "Sunshine", "Overcast", "Rain" };

        public async Task<WeatherModel> GetWeatherAsync(IProgress<int> progress)
        {
            progress.Report(0);

            // Simulate long-running data retrieving.
            var temperature = await Task.FromResult(_rand.Next(-20, 50));
            await Task.Delay(new TimeSpan(0, 0, 5));

            progress.Report(1);

            var wind = await Task.FromResult(_rand.Next(0, 10));
            await Task.Delay(new TimeSpan(0, 0, 5));

            progress.Report(2);

            var condition = await Task.FromResult(_conditions[_rand.Next(0, _conditions.Length - 1)]);
            await Task.Delay(new TimeSpan(0, 0, 5));

            progress.Report(3);

            return new WeatherModel { Temperature = temperature, Wind = wind, Condition = condition };
        }
    }
}