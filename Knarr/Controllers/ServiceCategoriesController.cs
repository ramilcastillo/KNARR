using AutoMapper;
using Knarr.Controllers.Resources.ServiceCategories;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ServiceCategoriesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceCategoryRepository _repository;

        public ServiceCategoriesController(IMapper mapper, IUnitOfWork unitOfWork, IServiceCategoryRepository repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        //GET api/servicecategories
        [HttpGet("{serviceTypeId}")]
        public async Task<IEnumerable<ServiceCategoryResource>> GetServiceCategoriesByServiceType(int serviceTypeId)
        {
            var serviceCategories = await _repository.GetServiceCategoriesByServiceTypeAsync(serviceTypeId);

            return _mapper.Map<IEnumerable<ServiceCategory>, IEnumerable<ServiceCategoryResource>>(serviceCategories);
        }

        [HttpGet]
        public async Task<IEnumerable<ServiceCategoryResource>> GetServiceCategories()
        {
            var serviceCategories = await _repository.GetServiceCategoriesAsync();

            return _mapper.Map<IEnumerable<ServiceCategory>, IEnumerable<ServiceCategoryResource>>(serviceCategories);
        }

        //GET api/servicecategories/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceCategory(int id)
        {
            var serviceCategory = await _repository.GetServiceCategoryAsync(id);

            if (serviceCategory == null)
                return NotFound();

            var result = _mapper.Map<ServiceCategory, ServiceCategoryResource>(serviceCategory);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMultipleServiceCategories([FromBody] SaveMultipleServiceCategories resource)
        {
            try
            {
                _repository.DeleteServiceServiceTypesByServiceId(resource.ServiceId);

                var serviceCategories = resource.ServiceCategoryId;
                var lstCategories = new List<ServiceServiceTypes>();
                foreach (var s in serviceCategories)
                {
                    var input = new ServiceServiceTypes
                    {
                        ServiceId = resource.ServiceId,
                        ServiceCategoryId = s,
                        ServiceTypeId  = resource.ServiceTypeId
                    };

                    _repository.AddServiceCategory(input);
                    lstCategories.Add(input);
                }

                return Ok(lstCategories);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message);
            }
        }

        //POST api/servicecategories
        [HttpPost]
        public async Task<IActionResult> CreateServiceCategory([FromBody] SaveServiceCategoryResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var serviceCategory = _mapper.Map<SaveServiceCategoryResource, ServiceCategory>(resource);

            _repository.Add(serviceCategory);

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map(serviceCategory, resource);

            return Ok(result);
        }



    }
}
