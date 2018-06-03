using AutoMapper;
using Knarr.Controllers.Resources.Services;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]/")]
    public class ServicesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceProviderRepository _serviceProviderRepository;

        public ServicesController(IMapper mapper, IServiceRepository repository, IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager,IServiceProviderRepository serviceProviderRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _serviceProviderRepository = serviceProviderRepository;
        }

        //POST api/services
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] SaveServicesResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            resource.ServiceProviderId = await GetServiceProviderId();
            var service = _mapper.Map<SaveServicesResource, Service>(resource);

            _repository.Add(service);

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map(service, resource);
            return Ok(result);

        }

        //GET api/services
        [HttpGet]
        public async Task<IActionResult> GetServices(string filterBy = null)
        {
            IActionResult result;
            if (string.IsNullOrWhiteSpace(filterBy))
            {
                var services = await _repository.GetServicesAsync();
                result = Ok(_mapper.Map<IEnumerable<Service>, IEnumerable<ServicesResource>>(services));
            }
            else
            {
                switch (filterBy.ToLower())
                {
                    case "serviceprovider":
                        var id = await GetServiceProviderId();
                        var services = await _repository.GetServicesByServiceProviderIdAsync(id);
                        result = Ok(_mapper.Map<IEnumerable<Service>, IEnumerable<ServicesResource>>(services));
                        break;
                    default:
                        result = BadRequest("The specified parameter is not valid");
                        break;
                }
            }

            return result;
        }

        //GET api/services/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _repository.GetServiceAsync(id);
            return Ok(_mapper.Map<Service, ServicesResource>(service));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SaveServicesResource resource)
        {
            IActionResult result;
            var serviceDetails = await _repository.GetServiceAsync(id);
            if (serviceDetails != null)
            {
                var mapperResult = _mapper.Map<SaveServicesResource,Service>(resource,serviceDetails);
                mapperResult.ServiceProviderId = await GetServiceProviderId();
                await _unitOfWork.CompleteAsync();
                result = Ok(mapperResult);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult result;
            var serviceDetails = await _repository.GetServiceAsync(id);
            if (serviceDetails != null)
            {
                serviceDetails.IsDeleted = true;
                await _unitOfWork.CompleteAsync();
                
                result = Ok();
            }
            else
            {
                result = NotFound("Can not find");
            }

            return result;
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
