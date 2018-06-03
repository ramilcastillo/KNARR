using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IInactiveDaysRepository
    {
        void Add(InactiveDay days);
        Task<IEnumerable<InactiveDay>> GetInactiveDaysAsync();
        Task<InactiveDay> GetInactiveDayAsync(int id);
        Task<IEnumerable<InactiveDay>> GetInactiveDayPerServiceAsync(int serviceId);
        void Delete(InactiveDay days);
    }
}
