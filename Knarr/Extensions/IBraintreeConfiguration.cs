using Braintree;

namespace Knarr.Extensions
{
    public interface IBraintreeConfiguration
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
    }
}
