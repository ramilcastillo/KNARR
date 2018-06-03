using System;
using AutoMapper;
using Knarr.Controllers.Resources.ServiceProviders;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ServiceProvidersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceProviderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceProviderRepository _serviceProviderRepository;

        public ServiceProvidersController(IMapper mapper,
            IServiceProviderRepository repository,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,IServiceProviderRepository serviceProviderRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _serviceProviderRepository = serviceProviderRepository;
        }

        //GET api/serviceproviders
        [HttpGet]
        public async Task<IEnumerable<ServiceProviderResource>> GetServiceProviders()
        {
            var sp = await _repository.GetServiceProvidersAsync();

            return _mapper.Map<IEnumerable<ServiceProvider>, IEnumerable<ServiceProviderResource>>(sp);
        }

        //GET api/serviceproviders/1
        [HttpGet("{id}")]
        public async Task<ServiceProviderResource> GetServiceProvider(int id)
        {
            var sp = await _repository.GetServiceProviderAsync(id);

            return _mapper.Map<ServiceProvider, ServiceProviderResource>(sp);
        }

        //GET api/ServiceProviders/getByUser
        [HttpGet]
        [ActionName("getByUser")]
        public async Task<IEnumerable<ServiceProviderResource>> GetServiceProvidersByUser()
        {
            var services = await _serviceProviderRepository.GetServiceProviderByUserAsync(await GetUserId());
            return _mapper.Map<IEnumerable<ServiceProvider>, IEnumerable<ServiceProviderResource>>(services);
        }

        //POST api/serviceproviders
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaveServiceProviderResource resource)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var sp = _mapper.Map<SaveServiceProviderResource, ServiceProvider>(resource);
                sp.UserId = await GetUserId();

                var spDetails = await _repository.GetServiceProviderByUserAsync(sp.UserId);
                if (spDetails.Any())
                {
                    return BadRequest("You can not add more than one service provider");
                }
                sp.IsAvailable = true;
                sp.ServiceProviderRequestsStatusId = (int)ServiceProviderStatusType.Submitted;
                _repository.Add(sp);

                await _unitOfWork.CompleteAsync();

                var result = _mapper.Map<ServiceProvider, ServiceProviderResource>(sp);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            
        }

        //UPDATE api/serviceproviders/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]SaveServiceProviderResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var sp = await _repository.GetServiceProviderAsync(id);
            if (sp == null)
                return NotFound();
            sp = _mapper.Map(resource, sp);
            sp.UserId = await GetUserId();
            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<SaveServiceProviderResource, ServiceProvider>(resource);

            return Ok(result);

        }

        //DELETE api/serviceproviders/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sp = await _repository.GetServiceProviderAsync(id);
            if (sp == null)
                return NotFound();
            _repository.Delete(sp);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateServiceProviderAvailability([FromBody]SaveServiceProviderResource resource) 
        {
            var sp = await _repository.GetServiceProviderAsync(await GetServiceProviderId());
            if (sp == null)
                return NotFound();
            
            sp.IsAvailable = resource.IsAvailable;
            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<ServiceProvider, SaveServiceProviderResource>(sp);

            return Ok(result);
        }

        [HttpPut("{spId}")]
        [ActionName("UpdateServiceProvidersRequestsStatus")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UpdateServiceProvidersRequestsStatus(int spId,[FromBody]SaveServiceProviderResource resource)
        {
            var sp = await _repository.GetServiceProviderAsync(spId);
            if (sp == null)
                return NotFound();

            sp.ServiceProviderRequestsStatusId = resource.ServiceProviderRequestsStatusId;

            await _unitOfWork.CompleteAsync();
            if (sp.ServiceProviderRequestsStatusId == (int)ServiceProviderStatusType.Approved)
            {
                await _userManager.AddToRoleAsync(sp.ApplicationUser, "Captain");
            }

            var result = _mapper.Map<ServiceProvider, SaveServiceProviderResource>(sp);

            return Ok(result);
        }

        [HttpGet("{statusId}")]
        [ActionName("GetServiceProvidersRequestsByStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<ServiceProviderResource>> GetServiceProvidersByStatus(int statusId)
        {
            var pendingRequests = await  _serviceProviderRepository.GetServiceProvidersByStatusAsync(statusId);
            var result =
                _mapper.Map<IEnumerable<ServiceProvider>, IEnumerable<ServiceProviderResource>>(pendingRequests);
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
