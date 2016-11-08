using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAsyncPack.Command
{
    public class ProgressiveAsyncCommand<TProgress> : AsyncCommand
    {
        private readonly Func<object, CancellationToken, IProgress<TProgress>, Task> _command;
        private readonly IProgress<TProgress> _progress;

        public ProgressiveAsyncCommand(
            Func<object, CancellationToken, IProgress<TProgress>, Task> command,
            Action<TProgress> progressHandler,
            Func<object, bool> canExecute = null)
            : base((Func<Task>)null, canExecute)
        {
            _command = command;
            _progress = new Progress<TProgress>(progressHandler);
        }

        public ProgressiveAsyncCommand(
            Func<object, IProgress<TProgress>, Task> command,
            Action<TProgress> progressHandler,
            Func<object, bool> canExecute = null)
            : this((param, token, progress) => command(param, progress), progressHandler, canExecute)
        {
        }

        public ProgressiveAsyncCommand(
            Func<IProgress<TProgress>, Task> command,
            Action<TProgress> progressHandler,
            Func<object, bool> canExecute = null)
            : this((param, token, progress) => command(progress), progressHandler, canExecute)
        {
        }

        protected override Task ExecuteCommand(object parameter, CancellationToken cancellationToken)
        {
            return _command(parameter, cancellationToken, _progress);
        }
    }
}