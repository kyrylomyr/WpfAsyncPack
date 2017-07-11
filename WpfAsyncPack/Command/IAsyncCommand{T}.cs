using System.Threading.Tasks;
using System.Windows.Input;
using WpfAsyncPack.Base;

namespace WpfAsyncPack.Command
{
    /// <summary>
    /// Defines an asynchronous command that supports cancellation and provides detailed information about execution completion.
    /// </summary>
    /// <typeparam name="T">The type of element used by command.</typeparam>
    public interface IAsyncCommand<in T> : ICommand
    {
        /// <summary>Defines the asynchronous method to be called when the command is invoked.</summary>
        /// <param name="parameter">Data used by the command.</param>
        Task ExecuteAsync(T parameter = default(T));

        /// <summary>
        /// Gets the detailed information about the command task status and completion.
        /// </summary>
        /// <value>
        /// The detailed information about the command task status and completion.
        /// </value>
        ObservableTask Task { get; }

        /// <summary>
        /// Defines the command that cancels execution of the <see cref="ExecuteAsync"/> method.
        /// </summary>
        /// <value>
        /// The cancellation command.
        /// </value>
        ICommand CancelCommand { get; }
    }
}