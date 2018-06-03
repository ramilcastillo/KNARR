using Braintree;
using Knarr.Core.Models.AppSettings;
using Microsoft.Extensions.Options;

namespace Knarr.Extensions
{
    public class BraintreeConfiguration : IBraintreeConfiguration
    {
        private readonly BraintreeAppSettings _options;
        private IBraintreeGateway BraintreeGateway { get; set; }

        public BraintreeConfiguration(IOptions<BraintreeAppSettings> options)
        {
            _options = options.Value;
        }
        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(Environment.SANDBOX, _options.MerchantId, _options.PublicKey, _options.PrivateKey);
        }

        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }
            return BraintreeGateway;
        }
    }
}
