using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfAsyncPack
{
    public class AsyncUiBase
    {
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

        public Task InvokeInUiThreadAsync(Action action)
        {
            return _dispatcher.InvokeAsync(action, DispatcherPriority.Normal).Task;
        }
    }
}
