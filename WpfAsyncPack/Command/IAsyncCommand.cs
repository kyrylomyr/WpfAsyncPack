namespace WpfAsyncPack.Command
{
    /// <summary>
    /// Defines an asynchronous command that supports cancellation and provides detailed information about execution completion.
    /// </summary>
    public interface IAsyncCommand : IAsyncCommand<object>
    {
    }
}