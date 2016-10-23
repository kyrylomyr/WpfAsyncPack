using System.Runtime.CompilerServices;

namespace WpfAsyncPack
{
    public abstract class BaseViewModel : PropertyChangeNotifiable
    {
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
