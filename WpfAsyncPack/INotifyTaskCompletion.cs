using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfAsyncPack
{
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