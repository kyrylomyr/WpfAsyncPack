using System;
using System.Threading.Tasks;

namespace WpfAsyncPack.Base
{
    /// <summary>
    /// Observes the task and notifies about status changes and completion result.
    /// </summary>
    public sealed class ObservableTask : AsyncBindableBase
    {
        /// <summary>
        /// Gets the task that is observed.
        /// </summary>
        /// <value>
        /// The task that is observed.
        /// </value>
        public Task Task { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the task is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning => Task != null && !Task.IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the task is not running. The value is opposite to the value of <see cref="IsRunning"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task is not running; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotRunning => !IsRunning;

        /// <summary>
        /// Gets a value indicating whether the task has been completed. Task is completed even if it is faulted.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has been completed; <c>false</c> if the task has not been completed or observed yet.
        /// </value>
        public bool IsCompleted => Task?.IsCompleted ?? false;

        /// <summary>
        /// Gets a value indicating whether the task has not been completed. The value is opposite to the value of <see cref="IsCompleted"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has not been completed or observed yet; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotCompleted => !IsCompleted;

        /// <summary>
        /// Gets a value indicating whether the task has been cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has been cancelled; <c>false</c> if the task has not been cancelled or observed yet.
        /// </value>
        public bool IsCanceled => Task?.IsCanceled ?? false;

        /// <summary>
        /// Gets a value indicating whether the task has not been cancelled. The value is opposite to the value of <see cref="IsCanceled"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has not been cancelled or observed yet; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotCanceled => !IsCanceled;

        /// <summary>
        /// Gets a value indicating whether the task has been completed successfully without exceptions.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has been completed successfully; <c>false</c> if the task has been completed
        /// with an exception, or has not been observed yet.
        /// </value>
        public bool IsSuccessfullyCompleted => Task != null && Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Gets a value indicating whether the task has been completed with a failure. The failure details are contained
        /// in the <see cref="Exception"/>, <see cref="InnerException"/> and <see cref="ErrorMessage"/> properties.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has been completed with a failure; <c>false</c> if it is not faulted or
        /// has not been observed yet.
        /// </value>
        public bool IsFaulted => Task?.IsFaulted ?? false;

        /// <summary>
        /// Gets the aggregated faulting exceptions for the task.
        /// </summary>
        /// <value>
        /// The aggregated faulting exceptions for the task. The value is <c>null</c> if task has been completed
        /// successfully or has not been observed yet.
        /// </value>
        public AggregateException Exception => Task?.Exception;

        /// <summary>
        /// Gets the original faulting exception for the task.
        /// </summary>
        /// <value>
        /// The original faulting exception for the task. The value is <c>null</c> if task has been completed
        /// successfully or has not been observed yet.
        /// </value>
        public Exception InnerException => Exception?.InnerException;

        /// <summary>
        /// Gets the error message of the original faulting exception for the task.
        /// </summary>
        /// <value>
        /// The error message of the original faulting exception for the task. The value is <c>null</c> if task
        /// has been completed successfully or has not been observed yet.
        /// </value>
        public string ErrorMessage => InnerException?.Message;

        /// <summary>
        /// Observes the specified task and notifies about status changes and completion result.
        /// </summary>
        /// <param name="task">The task to be observed.</param>
        /// <returns>The task-wrapper that does observation and notifies about changes.</returns>
        public async Task Observe(Task task)
        {
            Task = task;
            NotifyAllPropertiesChanged();

            try
            {
                await Task;
            }
            catch
            {
                // Skipped because we handle failures in bindable properties.
            }

            NotifyAllPropertiesChanged();
        }

        private void NotifyAllPropertiesChanged()
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChangedAsync(string.Empty);
        }
    }
}