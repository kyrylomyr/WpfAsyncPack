using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfAsyncPack
{
    /// <summary>
    /// Defines the <see cref="Task"/> wrapper that notifies about task execution completion.
    /// </summary>
    public interface INotifyTaskCompletion : INotifyPropertyChanged
    {
        Task Task { get; }

        Task TaskCompletion { get; }

        TaskStatus Status { get; }

        bool IsCompleted { get; }

        bool IsNotCompleted { get; }

        bool IsSuccessfullyCompleted { get; }

        bool IsCanceled { get; }

        bool IsFaulted { get; }

        AggregateException Exception { get; }

        Exception InnerException { get; }

        string ErrorMessage { get; }
    }
}