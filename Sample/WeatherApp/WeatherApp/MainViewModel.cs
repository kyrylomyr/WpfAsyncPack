using System;
using System.Threading.Tasks;
using WpfAsyncPack;

namespace WeatherApp
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly WeatherService _service;
        private int _temperature;
        private int _wind;
        private string _condition;

        public MainViewModel(WeatherService service)
        {
            _service = service;

            InitCommand = new AsyncCommand(
                async () =>
                {
                    // Some long-running initialization here.
                    await Task.Delay(new TimeSpan(0, 0, 10));
                });

            GetWeatherCommand = new AsyncCommand(
                async () =>
                {
                    // Call of the long-running external service.
                    var weather = await _service.GetWeatherAsync();

                    // Set the view model properties to display the result.
                    Temperature = weather.Temperature;
                    Wind = weather.Wind;
                    Condition = weather.Condition;
                },
                _ => !InitCommand.IsExecuting());
        }

        public MainViewModel() : this(new WeatherService())
        {
        }

        public int Temperature
        {
            get { return _temperature; }
            set { SetProperty(ref _temperature, value); }
        }

        public int Wind
        {
            get { return _wind; }
            set { SetProperty(ref _wind, value); }
        }

        public string Condition
        {
            get { return _condition; }
            set { SetProperty(ref _condition, value); }
        }

        public IAsyncCommand InitCommand { get; }

        public IAsyncCommand GetWeatherCommand { get; }
    }
}