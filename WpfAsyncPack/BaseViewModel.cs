using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAsyncPack
{
    /// <summary>
    /// Provides the base infrastructure for the view models such as asynchronous notification about property value changes.
    /// </summary>
    public abstract class BaseViewModel : AsyncBindableBase
    {
        /// <summary>
        /// Sets the new property value and asynchronously notifies about the change. If the new value is equal to the current one, the
        /// subscribers of the <see cref="INotifyPropertyChanged.PropertyChanged"/> are not notified.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="storage">The reference to the field that stores the property value.</param>
        /// <param name="value">The new value to be set.</param>
        /// <param name="propertyName">The name of the property. It is determined automatically on the compilation time if not set.</param>
        /// <returns>
        /// <c>true</c>, if the new value is different from the current one and it was changed; otherwise, <c>false</c>.
        /// </returns>
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
    }
}
