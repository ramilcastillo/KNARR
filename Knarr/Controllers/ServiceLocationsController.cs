using System;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.ServiceLocation;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Route("api/ServiceLocations")]
    public class ServiceLocationsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceLocationRepository _repository;
        private readonly IUnitOfWork _uniOfWork;

        public ServiceLocationsController(IMapper mapper, IServiceLocationRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = repository;
            _uniOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceLocation([FromBody]SaveServiceLocationResource location)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var locationInfo = await _repository.GetServiceCurrentLocationAsync(location.ServiceId);
                var result = _mapper.Map<SaveServiceLocationResource, ServiceLocation>(location);
                if (locationInfo == null)
                {
                    _repository.AddServiceLocation(result);
                }
                else
                {
                    _repository.UpdateServiceLocation(result);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{serviceId}")]
        public async Task<ServiceLocationResource> GetServiceLocation(int serviceId)
        {
            var location = await _repository.GetServiceCurrentLocationAsync(serviceId);
            return _mapper.Map<ServiceLocation, ServiceLocationResource>(location);
        }
    }
}