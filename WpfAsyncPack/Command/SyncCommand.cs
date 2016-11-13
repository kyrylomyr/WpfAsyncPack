using System;
using System.Windows.Input;

namespace WpfAsyncPack.Command
{
    /// <summary>
    /// A delegate command.
    /// </summary>
    public class SyncCommand : ICommand
    {
        /// <summary>
        /// The action executed by the command.
        /// </summary>
        protected Action<object> ExecuteAction;

        /// <summary>
        /// The function that determines if command can be executed.
        /// </summary>
        protected Func<object, bool> CanExecuteFunc;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The method with parameter. It is executed by the command.</param>
        /// <param name="canExecute">The method that determines whether the command can be executed in its current state or not.</param>
        public SyncCommand(
            Action<object> execute,
            Func<object, bool> canExecute = null)
        {
            ExecuteAction = execute;
            CanExecuteFunc = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncCommand"/> class. This constructor doesn't set the <see cref="ExecuteAction"/>
        /// and <see cref="CanExecuteFunc"/>. They should be set manually in the derived class before the command is executed.
        /// </summary>
        protected SyncCommand()
        {
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter = null)
        {
            ExecuteAction(parameter);
            RaiseCanExecuteChanged();
        }

        /// <summary>The method that determines whether the command can be executed in its current state or not.</summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns><c>true</c> if the command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter = null)
        {
            return CanExecuteFunc == null || CanExecuteFunc(parameter);
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event notifying the command state was changed.
        /// </summary>
        protected static void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
