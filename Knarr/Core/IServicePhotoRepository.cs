using Knarr.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Core
{
    public interface IServicePhotoRepository
    {
        Task<IEnumerable<ServicePhoto>> GetPhotos(int serviceId);
        Task<ServicePhoto> GetPhoto(int id);
        void Delete(ServicePhoto servicePhoto);
    }
}
