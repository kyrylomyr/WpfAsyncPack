using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAsyncPack.Base;
using WpfAsyncPack.Internal;

namespace WpfAsyncPack.Command
{
    /// <summary>
    /// An asynchronous delegate command that supports cancellation and provides bindable detailed information about execution completion.
    /// </summary>
    public class AsyncCommand : AsyncBindableBase, IAsyncCommand
    {
        /// <summary>
        /// The function executed by the command.
        /// </summary>
        protected Func<object, CancellationToken, Task> ExecuteFunc;

        /// <summary>
        /// The function that determines if command can be executed.
        /// </summary>
        protected Func<object, bool> CanExecuteFunc;

        private readonly CancelAsyncCommand _cancelCommand = new CancelAsyncCommand();

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The asynchronous method that accepts parameter and supports cancellation.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public AsyncCommand(
            Func<object, CancellationToken, Task> execute,
            Func<object, bool> canExecute = null)
        {
            ExecuteFunc = execute;
            CanExecuteFunc = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The asynchronous method that accepts parameter.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public AsyncCommand(
            Func<object, Task> execute,
            Func<object, bool> canExecute = null)
            : this((param, token) => execute(param), canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The asynchronous method.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public AsyncCommand(
            Func<Task> execute,
            Func<object, bool> canExecute = null)
            : this((param, token) => execute(), canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class. This constructor doesn't set the <see cref="ExecuteFunc"/>
        /// and <see cref="CanExecuteFunc"/> functions. They should be set manually in the derived class before the command is executed.
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
        public async void Execute(object parameter = null)
        {
            await ExecuteAsync(parameter);
        }

        /// <summary>
        /// Asynchronously executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public async Task ExecuteAsync(object parameter = null)
        {
            _cancelCommand.NotifyCommandStarting();
            var task = Task.Observe(ExecuteFunc(parameter, _cancelCommand.Token));
            RaiseCanExecuteChanged();
            await task;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        /// <summary>The method that determines whether the command can be executed in its current state or not.</summary>
        /// <param name="parameter">
        /// Data used by the command. If the execute does not require data to be passed, this object can be set to <c>null</c>.
        /// </param>
        /// <returns><c>true</c> if the command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter)
        {
            return Task.IsNotRunning && (CanExecuteFunc == null || CanExecuteFunc(parameter));
        }

        private static void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}