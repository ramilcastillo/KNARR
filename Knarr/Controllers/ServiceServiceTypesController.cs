using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.ServiceServiceTypes;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Route("api/[controller]")]
    public class ServiceServiceTypesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceServiceTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceServiceTypesController(IMapper mapper, IServiceServiceTypeRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceServiceType([FromBody] SaveServiceServiceTypeResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var source = _mapper.Map<SaveServiceServiceTypeResource, ServiceServiceTypes>(resource);
            _repository.AddServiceServiceType(source);
            await _unitOfWork.CompleteAsync();
            var result = _mapper.Map<ServiceServiceTypes, SaveServiceServiceTypeResource>(source);
            return Ok(result);
        }


        [HttpGet("{serviceId}")]
        public async Task<IEnumerable<ServiceServiceTypeResource>> GetServiceServiceTypesByServiceId(int serviceId)
        {
            var result = await _repository.GetServiceServiceTypeByServiceIdAsync(serviceId);
            return _mapper.Map<IEnumerable<ServiceServiceTypes>, IEnumerable<ServiceServiceTypeResource>>(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var info = await _repository.GetServiceServiceTypeAsync(id);
            if (info == null)
                return NotFound();
            _repository.DeleteServiceServiceType(info);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }
    }
}