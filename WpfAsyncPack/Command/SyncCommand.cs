using System;

namespace WpfAsyncPack.Command
{
    public class SyncCommand : SyncCommand<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The method with parameter. It is executed by the command.</param>
        /// <param name="canExecute">
        /// The method that determines whether the command can be executed in its current state or not.
        /// </param>
        public SyncCommand(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncCommand"/> class. This constructor doesn't set
        /// the <see cref="SyncCommand.ExecuteAction"/> and <see cref="SyncCommand.CanExecuteFunc"/>.
        /// They should be set manually in the derived class before the command is executed.
        /// </summary>
        protected SyncCommand()
        {
        }
    }
}