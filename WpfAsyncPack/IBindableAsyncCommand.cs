using System.Windows.Input;

namespace WpfAsyncPack
{
    public interface IBindableAsyncCommand<T> : IAsyncCommand
    {
        NotifyTaskCompletion<T> Execution { get; }

        ICommand CancelCommand { get; }
    }
}