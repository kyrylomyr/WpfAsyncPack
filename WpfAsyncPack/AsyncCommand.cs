using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsyncPack
{
    public class AsyncCommand : IAsyncCommand, INotifyPropertyChanged
    {
        private readonly Func<CancellationToken, Task> _command;
        private readonly Func<object, bool> _canExecute;

        private readonly CancelAsyncCommand _cancelCommand = new CancelAsyncCommand();

        private NotifyTaskCompletion _execution;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncCommand(Func<CancellationToken, Task> command, Func<object, bool> canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public AsyncCommand(Func<Task> command, Func<object, bool> canExecute = null) : this(t => command(), canExecute)
        {
        }

        public ICommand CancelCommand => _cancelCommand;

        public NotifyTaskCompletion Execution
        {
            get { return _execution; }
            private set
            {
                _execution = value;
                RaisePropertyChanged();
            }
        }

        public async void Execute(object parameter = null)
        {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter = null)
        {
            _cancelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion(_command(_cancelCommand.Token));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            return (Execution == null || Execution.IsCompleted) && (_canExecute == null || _canExecute(parameter));
        }

        public bool IsExecuting()
        {
            return Execution != null && !Execution.IsCompleted;
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
            private bool _commandExecuting;

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
                return _commandExecuting && !_cancellationTokenSource.IsCancellationRequested;
            }

            public void NotifyCommandStarting()
            {
                _commandExecuting = true;
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    RaiseCanExecuteChanged();
                }
            }

            public void NotifyCommandFinished()
            {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            private static void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}