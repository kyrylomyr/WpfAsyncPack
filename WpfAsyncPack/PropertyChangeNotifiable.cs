using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAsyncPack
{
    public abstract class PropertyChangeNotifiable : AsyncUiBase, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual async void RaisePropertyChangedAsync([CallerMemberName] string propertyName = null)
        {
            await InvokeInUiThreadAsync(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}
