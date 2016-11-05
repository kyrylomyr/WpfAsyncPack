using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfAsyncPack
{
    /// <summary>
    /// Provides asynchronous features to simplify implementation of classes bindable to the UI.
    /// </summary>
    public abstract class AsyncBindableBase : INotifyPropertyChanged
    {
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Asynchronously invokes the method in the UI thread using the current application's <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="action">The method to be invoked in the UI thread.</param>
        /// <returns>The task representing the method.</returns>
        public Task InvokeInUiThreadAsync(Action action)
        {
            return _dispatcher.InvokeAsync(action, DispatcherPriority.Normal).Task;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event that way so the subscribers are asynchronously invoked in the UI thread.
        /// </summary>
        /// <param name="propertyName">The name of the property. It is determined automatically on the compilation time if not set.</param>
        protected virtual async void RaisePropertyChangedAsync([CallerMemberName] string propertyName = null)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                await InvokeInUiThreadAsync(() => propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}
