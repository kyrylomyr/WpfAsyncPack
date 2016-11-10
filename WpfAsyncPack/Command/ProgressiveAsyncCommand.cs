using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAsyncPack.Command
{
    /// <summary>
    /// An asynchronous delegate command that supports <see cref="IProgress{T}" />, cancellation and provides bindable detailed information
    /// about execution completion.
    /// </summary>
    /// <typeparam name="TProgress">The type of the progress value.</typeparam>
    public class ProgressiveAsyncCommand<TProgress> : AsyncCommand
    {
        private readonly IProgress<TProgress> _progress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressiveAsyncCommand{TProgress}"/> class.
        /// </summary>
        /// <param name="execute">
        /// The asynchronous method that accepts parameter, an implementation of the <see cref="IProgress{TProgress}"/> to report
        /// the progress change and supports cancellation.
        /// </param>
        /// <param name="progressHandler">The progress change handler.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public ProgressiveAsyncCommand(
            Func<object, CancellationToken, IProgress<TProgress>, Task> execute,
            Action<TProgress> progressHandler,
            Func<object, bool> canExecute = null)
        {
            _progress = new Progress<TProgress>(progressHandler);
            ExecuteFunc = (param, token) => execute(param, token, _progress);
            CanExecuteFunc = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressiveAsyncCommand{TProgress}"/> class.
        /// </summary>
        /// <param name="execute">
        /// The asynchronous method that accepts parameter and an implementation of the <see cref="IProgress{TProgress}"/> to report
        /// the progress change.
        /// </param>
        /// <param name="progressHandler">The progress change handler.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public ProgressiveAsyncCommand(
            Func<object, IProgress<TProgress>, Task> execute,
            Action<TProgress> progressHandler,
            Func<object, bool> canExecute = null)
            : this((param, token, progress) => execute(param, progress), progressHandler, canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressiveAsyncCommand{TProgress}"/> class.
        /// </summary>
        /// <param name="execute">
        /// The asynchronous method that accepts an implementation of the <see cref="IProgress{TProgress}"/> to report the progress change.
        /// </param>
        /// <param name="progressHandler">The progress change handler.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public ProgressiveAsyncCommand(
            Func<IProgress<TProgress>, Task> execute,
            Action<TProgress> progressHandler,
            Func<object, bool> canExecute = null)
            : this((param, token, progress) => execute(progress), progressHandler, canExecute)
        {
        }
    }
}