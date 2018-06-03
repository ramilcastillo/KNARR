using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
