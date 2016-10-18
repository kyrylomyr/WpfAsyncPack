﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsyncPack
{
    public class AsyncCommand<T> : AsyncCommandBase, IBindableAsyncCommand<T>, INotifyPropertyChanged
    {
        private readonly Func<CancellationToken, Task<T>> _command;
        private readonly Func<object, bool> _canExecute;
        private readonly CancelAsyncCommand _cancelCommand = new CancelAsyncCommand();

        private NotifyTaskCompletion<T> _execution;

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand(Func<CancellationToken, Task<T>> command, Func<object, bool> canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public ICommand CancelCommand => _cancelCommand;

        public NotifyTaskCompletion<T> Execution
        {
            get { return _execution; }
            private set
            {
                _execution = value;
                RaisePropertyChanged();
            }
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _cancelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<T>(_command(_cancelCommand.Token));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            return (Execution == null || Execution.IsCompleted) && (_canExecute == null || _canExecute(parameter));
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

    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command)
        {
            return new AsyncCommand<object>(
                       async token =>
                       {
                           await command();
                           return null;
                       });
        }

        public static AsyncCommand<T> Create<T>(Func<Task<T>> command)
        {
            return new AsyncCommand<T>(token => command());
        }

        public static AsyncCommand<object> Create(Func<CancellationToken, Task> command)
        {
            return new AsyncCommand<object>(
                       async token =>
                       {
                           await command(token);
                           return null;
                       });
        }

        public static AsyncCommand<T> Create<T>(Func<CancellationToken, Task<T>> command)
        {
            return new AsyncCommand<T>(command);
        }

        public static AsyncCommand<object> Create(Func<Task> command, Func<object, bool> canExecute)
        {
            return new AsyncCommand<object>(
                       async token =>
                       {
                           await command();
                           return null;
                       },
                       canExecute);
        }

        public static AsyncCommand<T> Create<T>(Func<Task<T>> command, Func<object, bool> canExecute)
        {
            return new AsyncCommand<T>(token => command(), canExecute);
        }

        public static AsyncCommand<object> Create(Func<CancellationToken, Task> command, Func<object, bool> canExecute)
        {
            return new AsyncCommand<object>(
                       async token =>
                       {
                           await command(token);
                           return null;
                       },
                       canExecute);
        }

        public static AsyncCommand<T> Create<T>(Func<CancellationToken, Task<T>> command, Func<object, bool> canExecute)
        {
            return new AsyncCommand<T>(command, canExecute);
        }
    }
}