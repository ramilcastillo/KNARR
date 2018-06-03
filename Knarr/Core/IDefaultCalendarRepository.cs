using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IDefaultCalendarRepository
    {
        void Add(DefaultCalendar calendar);
        void Delete(DefaultCalendar calendar);
        Task<IEnumerable<DefaultCalendar>> GetDefaultCalenderByServiceProviderAsync(int serviceProviderId);
        Task<DefaultCalendar> GetdefaultCalendarAsync(int id);
    }
}
