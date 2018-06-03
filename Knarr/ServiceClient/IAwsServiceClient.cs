using System.Threading.Tasks;
using Knarr.Controllers;

namespace Knarr.ServiceClient
{
    public interface IAwsServiceClient
    {
        Task<string> UploadAsync(AwsServiceClientSettings awsServiceClientSettings);
    }
}