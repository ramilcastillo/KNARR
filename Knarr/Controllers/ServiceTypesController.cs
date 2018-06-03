using AutoMapper;
using Knarr.Controllers.Resources.ServiceTypes;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ServiceTypesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceTypesController(IMapper mapper, IServiceTypeRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        //GET api/servicetypes
        [HttpGet]
        public async Task<IEnumerable<ServiceTypeResource>> GetServiceTypesResource()
        {
            var st = await _repository.GetServiceTypesAsync();

            return _mapper.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeResource>>(st);
        }

        //GET api/servicetypes/1
        [HttpGet("{id}")]
        public async Task<ServiceTypeResource> GetServiceTypeResource(int id)
        {
            var st = await _repository.GetServiceTypeAsync(id);

            return _mapper.Map<ServiceType, ServiceTypeResource>(st);
        }

        //POST api/servicetypes
        [HttpPost]
        public async Task<IActionResult> CreateServiceType([FromBody] SaveServiceTypeResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var st = _mapper.Map<SaveServiceTypeResource, ServiceType>(resource);

            _repository.Add(st);

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<ServiceType, SaveServiceTypeResource>(st);

            return Ok(result);
        }

        //PUT api/servicetypes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceType(int id, [FromBody] SaveServiceTypeResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var st = await _repository.GetServiceTypeAsync(id);

            if (st == null)
                return NotFound();

            st = _mapper.Map(resource, st);

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<ServiceType, SaveServiceTypeResource>(st);

            return Ok(result);

        }

        //DELETE api/servicetypes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceType(int id)
        {
            var st = await _repository.GetServiceTypeAsync(id);

            if (st == null)
                return NotFound();

            _repository.Delete(st);

            await _unitOfWork.CompleteAsync();

            return Ok(id);

        }
    }
}
