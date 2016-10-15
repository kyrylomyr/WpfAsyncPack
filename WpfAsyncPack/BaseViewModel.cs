using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace WpfAsyncPack
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;

            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChangedAsync(propertyName);

            return true;
        }

        protected virtual async void RaisePropertyChangedAsync([CallerMemberName] string propertyName = null)
        {
            // Run in UI thread.
            await _dispatcher.InvokeAsync(
                () => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)),
                DispatcherPriority.Normal);
        }
    }
}
