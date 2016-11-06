using System;
using System.Threading.Tasks;
using WpfAsyncPack.Base;
using WpfAsyncPack.Command;

namespace WeatherApp
{
    internal class MainViewModel : AsyncBindableBase
    {
        private readonly WeatherService _service;
        private readonly Progress<int> _progress;

        private int _temperature;
        private int _wind;
        private string _condition;
        private int _updateWeatherProgress;

        public MainViewModel(WeatherService service)
        {
            _service = service;

            _progress = new Progress<int>(x => { UpdateWeatherProgress = x; });

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
                                        var weather = await _service.GetWeatherAsync(_progress);

                                        // Set the view model properties to display the result.
                                        Temperature = weather.Temperature;
                                        Wind = weather.Wind;
                                        Condition = weather.Condition;
                                    },
                                    _ => InitCommand.Task.IsNotRunning);
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

        public int UpdateWeatherProgress
        {
            get { return _updateWeatherProgress; }
            set { SetProperty(ref _updateWeatherProgress, value); }
        }

        public IAsyncCommand InitCommand { get; }

        public IAsyncCommand GetWeatherCommand { get; }
    }
}