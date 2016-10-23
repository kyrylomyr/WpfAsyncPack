using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfAsyncPack
{
    /// <summary>
    /// Defines the class that watches the task and notifies about its execution completion.
    /// </summary>
    public interface INotifyTaskCompletion : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the task that is watched.
        /// </summary>
        /// <value>
        /// The task that is watched.
        /// </value>
        Task Task { get; }

        /// <summary>
        /// Gets the wrapping task that watches the original task and notifies about its completion.
        /// </summary>
        /// <value>
        /// The wrapping task.
        /// </value>
        Task TaskCompletion { get; }

        /// <summary>
        /// Gets the task status.
        /// </summary>
        /// <value>
        /// The task status.
        /// </value>
        TaskStatus Status { get; }

        /// <summary>
        /// Gets a value indicating whether the task has completed or not. Task is completed even when it has failed with the exception.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has completed; otherwise, <c>false</c>.
        /// </value>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets a value indicating whether the task has not completed. The value is opposite to the value of <see cref="IsCompleted"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has not completed; otherwise, <c>false</c>.
        /// </value>
        bool IsNotCompleted { get; }

        /// <summary>
        /// Gets a value indicating whether the task has completed successfully without exceptions.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has completed successfully; otherwise, <c>false</c>.
        /// </value>
        bool IsSuccessfullyCompleted { get; }

        /// <summary>
        /// Gets a value indicating whether the task has been cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has been cancelled; otherwise, <c>false</c>.
        /// </value>
        bool IsCanceled { get; }

        /// <summary>
        /// Gets a value indicating whether the task has completed with a failure. The failure details are contained in the
        /// <see cref="Exception"/>, <see cref="InnerException"/> and <see cref="ErrorMessage"/> properties.
        /// </summary>
        /// <value>
        /// <c>true</c> if the task has completed with a failure; otherwise, <c>false</c>.
        /// </value>
        bool IsFaulted { get; }

        /// <summary>
        /// Gets the aggregated faulting exceptions for the task.
        /// </summary>
        /// <value>
        /// The aggregated faulting exceptions for the task. The value is <c>null</c> if task has completed successfully.
        /// </value>
        AggregateException Exception { get; }

        /// <summary>
        /// Gets the original faulting exception for the task.
        /// </summary>
        /// <value>
        /// The original faulting exception for the task. The value is <c>null</c> if task has completed successfully.
        /// </value>
        Exception InnerException { get; }

        /// <summary>
        /// Gets the error message of the original faulting exception for the task.
        /// </summary>
        /// <value>
        /// The error message of the original faulting exception for the task. The value is <c>null</c> if task has completed successfully.
        /// </value>
        string ErrorMessage { get; }
    }
}