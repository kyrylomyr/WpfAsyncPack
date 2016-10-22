using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfAsyncPack
{
    public sealed class NotifyTaskCompletion : INotifyTaskCompletion
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyTaskCompletion(Task task)
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

            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }

            propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotCompleted)));

            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Exception)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(InnerException)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
            }
        }
    }
}