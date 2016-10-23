using System;
using System.Threading.Tasks;

namespace WpfAsyncPack
{
    public sealed class TaskCompletionNotifier : PropertyChangeNotifiable, INotifyTaskCompletion
    {
        public TaskCompletionNotifier(Task task)
        {
            Task = task;
            TaskCompletion = WatchTaskAsync(task);
        }

        public Task Task { get; }

        public Task TaskCompletion { get; }

        public TaskStatus Status => Task.Status;

        public bool IsCompleted => Task.IsCompleted;

        public bool IsNotCompleted => !Task.IsCompleted;

        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        public bool IsCanceled => Task.IsCanceled;

        public bool IsFaulted => Task.IsFaulted;

        public AggregateException Exception => Task.Exception;

        public Exception InnerException => Exception?.InnerException;

        public string ErrorMessage => InnerException?.Message;

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // Skipped because we handle failures below.
            }

            // ReSharper disable ExplicitCallerInfoArgument
            RaisePropertyChangedAsync(nameof(Status));
            RaisePropertyChangedAsync(nameof(IsCompleted));
            RaisePropertyChangedAsync(nameof(IsNotCompleted));
            // ReSharper restore ExplicitCallerInfoArgument

            if (task.IsCanceled)
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChangedAsync(nameof(IsCanceled));
            }
            else if (task.IsFaulted)
            {
                // ReSharper disable ExplicitCallerInfoArgument
                RaisePropertyChangedAsync(nameof(IsFaulted));
                RaisePropertyChangedAsync(nameof(Exception));
                RaisePropertyChangedAsync(nameof(InnerException));
                RaisePropertyChangedAsync(nameof(ErrorMessage));
                // ReSharper restore ExplicitCallerInfoArgument
            }
            else
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChangedAsync(nameof(IsSuccessfullyCompleted));
            }
        }
    }
}