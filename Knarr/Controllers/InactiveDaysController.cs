using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.InactiveDays;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class InactiveDaysController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IInactiveDaysRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public InactiveDaysController(IMapper mapper, IInactiveDaysRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<InactiveDaysResource>> GetInactiveDays()
        {
            var result = await _repository.GetInactiveDaysAsync();
            return _mapper.Map<IEnumerable<InactiveDay>, IEnumerable<InactiveDaysResource>>(result);
        }

        [HttpGet("{serviceId}")]
        public async Task<IEnumerable<InactiveDaysResource>> GetInactiveDays(int serviceId)
        {
            var inactiveDays = await _repository.GetInactiveDayPerServiceAsync(serviceId);
            return _mapper.Map<IEnumerable<InactiveDay>, IEnumerable<InactiveDaysResource>>(inactiveDays);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInactiveDays([FromBody] SaveInactiveDaysResource[] resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            foreach (var res in resource)
            {
                var result = _mapper.Map<SaveInactiveDaysResource, InactiveDay>(res);
                _repository.Add(result);
                await _unitOfWork.CompleteAsync();
            }
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inactiveDay = await _repository.GetInactiveDayAsync(id);
            if (inactiveDay == null)
                return NotFound();

            _repository.Delete(inactiveDay);

            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }
    }
}