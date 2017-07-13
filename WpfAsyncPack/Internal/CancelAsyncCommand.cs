using System;
using System.Threading;
using System.Windows.Input;

namespace WpfAsyncPack.Internal
{
    internal sealed class CancelAsyncCommand : ICommand
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool _isCommandExecuting;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CancellationToken Token => _cancellationTokenSource.Token;

        void ICommand.Execute(object parameter)
        {
            _cancellationTokenSource.Cancel();
            RaiseCanExecuteChanged();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return _isCommandExecuting && !_cancellationTokenSource.IsCancellationRequested;
        }

        public void NotifyCommandStarting()
        {
            _isCommandExecuting = true;
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }
        }

        public void NotifyCommandFinished()
        {
            _isCommandExecuting = false;
            RaiseCanExecuteChanged();
        }

        private static void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}