using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAsyncPack.Command
{
    public class ProgressiveAsyncCommand<TProgress> : AsyncCommand
    {
        private readonly IProgress<TProgress> _progress;

        public ProgressiveAsyncCommand(
            Func<object, CancellationToken, IProgress<TProgress>, Task> command,
            Action<TProgress> progressHandler,
            Func<object, bool> canExecute = null)
        {
            _progress = new Progress<TProgress>(progressHandler);
            CommandFunc = (param, token) => command(param, token, _progress);
            CanExecuteFunc = canExecute;
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
    }
}