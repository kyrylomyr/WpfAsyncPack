using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAsyncPack.Base;
using WpfAsyncPack.Internal;

namespace WpfAsyncPack.Command
{
    /// <summary>
    /// An asynchronous delegate command that supports cancellation and provides bindable detailed information
    /// about execution completion.
    /// </summary>
    /// <typeparam name="T">The type of element used by command.</typeparam>
    public class AsyncCommand<T> : AsyncBindableBase, IAsyncCommand<T>
    {
        /// <summary>
        /// The function executed by the command.
        /// </summary>
        protected Func<T, CancellationToken, Task> ExecuteFunc;

        /// <summary>
        /// The function that determines if command can be executed.
        /// </summary>
        protected Func<T, bool> CanExecuteFunc;

        private readonly CancelAsyncCommand _cancelCommand = new CancelAsyncCommand();

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">
        /// The asynchronous method with parameter and cancellation support. It is executed by the command.
        /// </param>
        /// <param name="canExecute">
        /// The method that determines whether the command can be executed in its current state or not.
        /// </param>
        public AsyncCommand(
            Func<T, CancellationToken, Task> execute,
            Func<T, bool> canExecute = null)
        {
            ExecuteFunc = execute;
            CanExecuteFunc = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The asynchronous method with parameter. It is executed by the command.</param>
        /// <param name="canExecute">
        /// The method that determines whether the command can be executed in its current state or not.
        /// </param>
        public AsyncCommand(
            Func<T, Task> execute,
            Func<T, bool> canExecute = null)
            : this((param, token) => execute(param), canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand{T}"/> class. This constructor doesn't set
        /// the <see cref="ExecuteFunc"/> and <see cref="CanExecuteFunc"/> functions. They should be set manually
        /// in the derived class before the command is executed.
        /// </summary>
        protected AsyncCommand()
        {
        }

        /// <summary>
        /// Gets the detailed information about the command task status and completion.
        /// </summary>
        /// <value>
        /// The detailed information about the command task status and completion.
        /// </value>
        public ObservableTask Task { get; } = new ObservableTask();

        /// <summary>
        /// Defines the command that cancels execution of the <see cref="ExecuteAsync"/> method.
        /// </summary>
        /// <value>
        /// The cancellation command.
        /// </value>
        public ICommand CancelCommand => _cancelCommand;

        /// <summary>
        /// Executes the command. Internally the execute will be executed asynchronously.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public async void Execute(T parameter = default(T))
        {
            await ExecuteAsync(parameter);
        }

        /// <summary>
        /// Asynchronously executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public async Task ExecuteAsync(T parameter = default(T))
        {
            _cancelCommand.NotifyCommandStarting();
            var task = Task.Observe(ExecuteFunc(parameter, _cancelCommand.Token));
            RaiseCanExecuteChanged();
            await task;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        /// <summary>The method that determines whether the command can be executed in its current state or not.</summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns><c>true</c> if the command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanExecute(T parameter = default(T))
        {
            return Task.IsNotRunning && (CanExecuteFunc == null || CanExecuteFunc(parameter));
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event notifying the command state was changed.
        /// </summary>
        protected static void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }
}