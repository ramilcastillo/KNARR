using AutoMapper;
using Knarr.Controllers.Resources.ServiceProviderTypes;
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
    public class ServiceProviderTypesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProviderTypeRepository _repository;

        public ServiceProviderTypesController(IMapper mapper, IUnitOfWork unitOfWork, IServiceProviderTypeRepository repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        //GET api/serviceprovidertypes
        [HttpGet]
        public async Task<IEnumerable<ServiceProviderTypeResource>> GetServiceProviderTypes()
        {
            var spt = await _repository.GetServiceProviderTypesAsync();

            return _mapper.Map<IEnumerable<ServiceProviderType>, IEnumerable<ServiceProviderTypeResource>>(spt);
        }

        //GET api/serviceprovidertypes/1
        [HttpGet("{id}")]
        public async Task<IEnumerable<ServiceProviderTypeResource>> GetServiceProviderType(int id)
        {
            var spt = await _repository.GetServiceProviderTypeAsync(id);

            return _mapper.Map<IEnumerable<ServiceProviderType>, IEnumerable<ServiceProviderTypeResource>>(spt);

        }

        //POST api/serviceprovidertypes
        [HttpPost]
        public async Task<IActionResult> CcreateServiceProviderType([FromBody] SaveServiceProviderTypeResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var spt = _mapper.Map<SaveServiceProviderTypeResource, ServiceProviderType>(resource);

            _repository.Add(spt);

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map(spt, resource);
            return Ok(result);
        }

        //DELETE api/serviceprovidertypes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> UpdateServiceProviderType(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spt = await _repository.GetServiceProviderTypeByIdAsync(id);

            if (spt == null)
                return NotFound();
            _repository.Delete(spt);

            await _unitOfWork.CompleteAsync();
            
            return Ok(id);

        }
    }
}
