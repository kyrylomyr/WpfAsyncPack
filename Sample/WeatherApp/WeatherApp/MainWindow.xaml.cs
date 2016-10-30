using System.Windows;

namespace WeatherApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            mainViewModel.InitCommand.ExecuteAsync();
        }
    }
}
