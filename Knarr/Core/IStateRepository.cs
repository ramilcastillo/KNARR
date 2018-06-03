using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IStateRepository
    {
        Task<IEnumerable<States>> GetAllStatesAsync(); 
    }
}
