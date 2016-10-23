using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfAsyncPack
{
    /// <summary>
    /// Provides base asynchronous features for working with UI such as executing code in UI thread.
    /// </summary>
    public class AsyncUiBase
    {
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

        /// <summary>
        /// Asynchronously invokes the method in the UI thread using the current application's <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="action">The method to be invoked in the UI thread.</param>
        /// <returns>The task representing the method.</returns>
        public Task InvokeInUiThreadAsync(Action action)
        {
            return _dispatcher.InvokeAsync(action, DispatcherPriority.Normal).Task;
        }
    }
}
