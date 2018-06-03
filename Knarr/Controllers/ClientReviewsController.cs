using AutoMapper;
using Knarr.Controllers.Resources.ClientReviews;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ClientReviewsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IClientReviewRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceProviderRepository _serviceProviderRepository;

        public ClientReviewsController(IMapper mapper, IClientReviewRepository repository, IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager,IServiceProviderRepository serviceProviderRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _serviceProviderRepository = serviceProviderRepository;
        }

        //POST api/ClientReviews
        [HttpPost]
        public async Task<IActionResult>  ate([FromBody]SaveClientReviewsResource reviews)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            reviews.ServiceProviderId = await GetServiceProviderId();
            var resources = _mapper.Map<SaveClientReviewsResource, ClientReviews>(reviews);
            _repository.AddClientReviewAsync(resources);
            await _unitOfWork.CompleteAsync();
            var result = _mapper.Map<ClientReviews, SaveClientReviewsResource>(resources);
            return Ok(result);
        }

        //GET api/ClientReviews
        [HttpGet]
        public async Task<IEnumerable<ClientReviewsRerource>> GetClientReviews()
        {
            var id = await GetUserId();
            var result = await _repository.GetClientReviewAsync(id);
            return _mapper.Map<IEnumerable<ClientReviews>, IEnumerable<ClientReviewsRerource>>(result);
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<ClientReviewsRerource>> GetSpecificClientReviews(string userId)
        {
            var result = await _repository.GetClientReviewAsync(userId);
            return _mapper.Map<IEnumerable<ClientReviews>, IEnumerable<ClientReviewsRerource>>(result);
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ClientReviewsRerource>> GetClientReviews(string id)
        {
            var result = await _repository.GetClientReviewAsync(id);
            return _mapper.Map<IEnumerable<ClientReviews>, IEnumerable<ClientReviewsRerource>>(result);
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }

        private async Task<int> GetServiceProviderId()
        {
            var userId = await GetUserId();
            var serviceProviderDetails = await _serviceProviderRepository.GetServiceProviderDetailsByUserAsync(userId);
            return serviceProviderDetails.Id;
        }
    }
}