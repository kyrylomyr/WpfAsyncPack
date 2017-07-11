using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAsyncPack.Command
{
    /// <summary>
    /// An asynchronous delegate command that supports cancellation and provides bindable detailed information about execution completion.
    /// </summary>
    public class AsyncCommand : AsyncCommand<object>, IAsyncCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The asynchronous method with parameter and cancellation support. It is executed by the command.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public AsyncCommand(Func<object, CancellationToken, Task> execute, Func<object, bool> canExecute = null)
            : base(execute, canExecute)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The asynchronous method with parameter. It is executed by the command.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
            : base(execute, canExecute)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class. This constructor doesn't set the <see cref="AsyncCommand.ExecuteFunc"/>
        /// and <see cref="AsyncCommand.CanExecuteFunc"/> functions. They should be set manually in the derived class before the command is executed.
        /// </summary>
        protected AsyncCommand()
        {
        }
    }
}