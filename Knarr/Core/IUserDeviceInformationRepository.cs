using System.Collections.Generic;
using System.Threading.Tasks;
using Knarr.Core.Models;

namespace Knarr.Core
{
    public interface IUserDeviceInformationRepository
    {
        void Add(UserDeviceInformations information);
        Task<IEnumerable<UserDeviceInformations>> GetUserDeviceInformationsAsync(string userId);
    }
}
