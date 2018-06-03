using System.Security.Claims;
using System.Threading.Tasks;
using Braintree;
using Knarr.Core.Models;
using Knarr.Core.Models.Payments;
using Knarr.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly IBraintreeGateway _gateway;
        private readonly IBraintreeConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsController(IBraintreeConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _gateway = configuration.GetGateway();
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactions([FromBody]BraintreeTransactions request)
        {
            var knarrAmount = request.totalAmount * 0.8M;
            var thriftmatchAmount = request.totalAmount * 0.2M;

            var transRequest = new TransactionRequest
            {
                CustomerId = await UserId(),
                Amount = knarrAmount,
                MerchantAccountId = "knarr"

            };
            var transRequest2 = new TransactionRequest
            {
                CustomerId = await UserId(),
                Amount = thriftmatchAmount,
                MerchantAccountId = "thriftmatch"

            };
            var result = _gateway.Transaction.Sale(transRequest);
            var result2 = _gateway.Transaction.Sale(transRequest2);

            if (!result.IsSuccess())
                return BadRequest((result.Message));
            if (!result2.IsSuccess())
                return BadRequest(result2.Message);

            return Ok("Transaction is done");

        }

        private async Task<string> UserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }
    }
}
