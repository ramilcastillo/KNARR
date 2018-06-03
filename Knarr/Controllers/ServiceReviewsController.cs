using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.ServiceReviews;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Route("api/[controller]")]
    public class ServiceReviewsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceReviewRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceReviewsController(IMapper mapper, IServiceReviewRepository repository, IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("{serviceId}")]
        public async Task<IEnumerable<ServiceReviewResource>> GetReviewsByService(int serviceId)
        {
            var result = await _repository.GetReviews(serviceId);

            return _mapper.Map<IEnumerable<ServiceReview>, IEnumerable<ServiceReviewResource>>(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceReview([FromBody] SaveServiceReviewResource review)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var userInfo = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            var result = _mapper.Map<SaveServiceReviewResource, ServiceReview>(review);
            result.UserId = userInfo.Id;
            

            _repository.AddReview(result);
            await _unitOfWork.CompleteAsync();
            return Ok(result);
        }
    }
}