using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsyncPack
{
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public abstract Task ExecuteAsync(object parameter);

        public abstract bool CanExecute(object parameter);

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}