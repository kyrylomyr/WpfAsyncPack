using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfAsyncPack.Base
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
        /// Sets the new property value and asynchronously notifies about the change. If the new value is equal to the
        /// current one, the subscribers of the <see cref="INotifyPropertyChanged.PropertyChanged"/> are not notified.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="storage">The reference to the field that stores the property value.</param>
        /// <param name="value">The new value to be set.</param>
        /// <param name="propertyName">
        /// The name of the property. It is determined automatically on the compilation time if not set.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the new value is different from the current one and it was changed; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
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

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event that way so the subscribers are asynchronously invoked
        /// in the UI thread.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property. It is determined automatically on the compilation time if not set.
        /// </param>
        protected virtual async void RaisePropertyChangedAsync([CallerMemberName] string propertyName = "")
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                await InvokeInUiThreadAsync(
                    () => propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event that way so the subscribers are asynchronously invoked
        /// in the UI thread.
        /// </summary>
        /// <param name="propertyNames">The names of the properties.</param>
        protected virtual async void RaisePropertyChangedAsync(params string[] propertyNames)
        {
            if (propertyNames == null)
            {
                return;
            }

            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                foreach (var propertyName in propertyNames.AsParallel())
                {
                    await InvokeInUiThreadAsync(
                        () => propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
                }
            }
        }

        /// <summary>
        /// Asynchronously invokes the method in the UI thread using the current application's <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="action">The method to be invoked in the UI thread.</param>
        /// <returns>The task representing the method.</returns>
        protected Task InvokeInUiThreadAsync(Action action)
        {
            return _dispatcher.InvokeAsync(action, DispatcherPriority.Normal).Task;
        }
    }
}