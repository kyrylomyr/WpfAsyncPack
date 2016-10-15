using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsyncPack
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}