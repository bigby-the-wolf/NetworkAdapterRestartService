using System.Threading.Tasks;

namespace NetworkAdapterRestartService.Interfaces
{
    public interface IConnectionValidator
    {
        Task ValidateConnection();
    }
}