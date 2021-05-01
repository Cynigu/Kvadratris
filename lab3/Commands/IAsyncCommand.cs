using System.Threading.Tasks;
using System.Windows.Input;

namespace lab3.VM
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
