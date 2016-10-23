using System;
using System.Threading.Tasks;

namespace WpfAsyncPack
{
    /// <summary>
    /// Watches the task and notifies about its execution completion.
    /// </summary>
    public sealed class TaskCompletionNotifier : PropertyChangeNotifiable, INotifyTaskCompletion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCompletionNotifier"/> class.
        /// </summary>
        /// <param name="task">The task which completion should be watched and notified.</param>
        public TaskCompletionNotifier(Task task)
        {
            Task = task;
            TaskCompletion = WatchTaskAsync(task);
        }

        /// <summary>
        /// Gets the task that is watched.
        /// </summary>
        /// <value>
        /// The task that is watched.
        /// </value>
        public Task Task { get; }

        /// <summary>
        /// Gets the wrapping task that watches the original task and notifies about its completion.
        /// </summary>
        /// <value>
        /// The wrapping task.
        /// </value>
        public Task TaskCompletion { get; }

        /// <summary>
        /// Gets the task status.
        /// </summary>
        /// <value>
        /// The task status.
        /// </value>
        public TaskStatus Status => Task.Status;

        /// <summary>
        /// Gets a value indicating whether the task has completed or not. Task is completed even when it has failed with the exception.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompleted => Task.IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the task has not completed. The value is opposite to the value of <see cref="IsCompleted"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has not completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotCompleted => !Task.IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the task has completed successfully without exceptions.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has completed successfully; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Gets a value indicating whether the task has been cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has been cancelled; otherwise, <c>false</c>.
        /// </value>
        public bool IsCanceled => Task.IsCanceled;

        /// <summary>
        /// Gets a value indicating whether the task has completed with a failure. The failure details are contained in the
        /// <see cref="Exception"/>, <see cref="InnerException"/> and <see cref="ErrorMessage"/> properties.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has completed with a failure; otherwise, <c>false</c>.
        /// </value>
        public bool IsFaulted => Task.IsFaulted;

        /// <summary>
        /// Gets the aggregated faulting exceptions for the task.
        /// </summary>
        /// <value>
        /// The aggregated faulting exceptions for the task. The value is <c>null</c> if task has completed successfully.
        /// </value>
        public AggregateException Exception => Task.Exception;

        /// <summary>
        /// Gets the original faulting exception for the task.
        /// </summary>
        /// <value>
        /// The original faulting exception for the task. The value is <c>null</c> if task has completed successfully.
        /// </value>
        public Exception InnerException => Exception?.InnerException;

        /// <summary>
        /// Gets the error message of the original faulting exception for the task.
        /// </summary>
        /// <value>
        /// The error message of the original faulting exception for the task. The value is <c>null</c> if task has completed successfully.
        /// </value>
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