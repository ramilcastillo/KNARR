using Braintree;
using Knarr.Core.Models.Payments;
using Knarr.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Knarr.Core.Models;

namespace Knarr.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IBraintreeGateway _gateway;
        private readonly IBraintreeConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomersController(IBraintreeConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _gateway = configuration.GetGateway();
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]BraintreeNewCustomer customer)
        {
            var request = new CustomerRequest
            {
                Id = await UserId(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = ClaimTypes.Email

            };

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = await UserId(),
                Number = customer.CardNumber,
                ExpirationDate = customer.ExpirationDate,
                CVV = customer.CVV,
                CardholderName = customer.CardHolderName,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                },
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PostalCode = customer.PostalCode,
                    CountryName = customer.CountryName,
                    StreetAddress = customer.StreetAddress,
                    Region = customer.Region,
                    Options = new CreditCardAddressOptionsRequest
                    {
                        UpdateExisting = true
                    }

                }
            };

            var result = _gateway.Customer.Create(request);
            if (result.IsSuccess())
            {
                var ccResult = _gateway.CreditCard.Create(creditCardRequest);
                if (ccResult.IsSuccess())
                    return Ok(request);
                return BadRequest(ccResult.Message);

            }
            return BadRequest(result.Message);
        }

        [HttpGet()]
        public async Task<IActionResult> GetCustomer()
        {
            var customer = _gateway.Customer.Find(await UserId());

            return Ok(customer);
        }

        private async Task<string> UserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }
    }
}
