using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAsyncPack
{
    /// <summary>
    /// Represents the base for the class that can notifies about changes in its properties.
    /// </summary>
    public abstract class PropertyChangeNotifiable : AsyncUiBase, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event that way so the subscribers are asynchronously invoked in the UI thread.
        /// </summary>
        /// <param name="propertyName">The name of the property. It is determined automatically on the compilation time if not set.</param>
        protected virtual async void RaisePropertyChangedAsync([CallerMemberName] string propertyName = null)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                await ExecuteInUiThreadAsync(() => propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}
