using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsyncPack
{
    /// <summary>
    /// Defines an asynchronous command that supports cancellation and provides detailed information about execution completion.
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>Defines the asynchronous method to be called when the command is invoked.</summary>
        /// <param name="parameter">Data used by the command.</param>
        Task ExecuteAsync(object parameter = null);

        INotifyTaskCompletion Execution { get; }

        /// <summary>
        /// Defines the command that cancels execution of the <see cref="ExecuteAsync"/> method.
        /// </summary>
        /// <value>
        /// The cancellation command.
        /// </value>
        ICommand CancelCommand { get; }

        /// <summary>
        /// Defines the method that determines whether the command is executing or not.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if the command is executing; otherwise, <c>false</c>.
        /// </returns>
        bool IsExecuting();
    }
}