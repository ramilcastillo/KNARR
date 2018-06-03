using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.DefaultCalendars;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DefaultCalendarsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDefaultCalendarRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IServiceProviderRepository _serviceProviderRepository;

        public DefaultCalendarsController(IMapper mapper, IDefaultCalendarRepository repository, IUnitOfWork unitOfWork, IServiceProviderRepository serviceProviderRepository, UserManager<ApplicationUser> manager)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _manager = manager;
            _serviceProviderRepository = serviceProviderRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateDefaultCalendar([FromBody] SaveDefaultCalendarResource[] resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var serviceProviderDetails = await _serviceProviderRepository.GetServiceProviderDetailsByUserAsync(await GetUserId());

            var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month,1);
            var endDate = DateTime.UtcNow.AddMonths(12);

            for(var day = startDate.Date ; day <= endDate.Date ; day = day.AddDays(1))
            {
                var status = false;
                foreach (var res in resource)
                {
                    if ((int)day.DayOfWeek == res.day)
                    {
                        status = true;
                    }
                }
                if(!status)
                {
                    var calenderDetails = new DefaultCalendar();
                    calenderDetails.DefaultDates = day;
                    calenderDetails.ServiceProviderId = serviceProviderDetails.Id; 
                    _repository.Add(calenderDetails);

                    await _unitOfWork.CompleteAsync();
                }
            }
            return Ok();

        }

        [HttpGet]
        public async Task<IEnumerable<DefaultCalendarResource>> GetDefaultCalanders()
        {
            var serviceProviderDetails = await _serviceProviderRepository.GetServiceProviderDetailsByUserAsync(await GetUserId());

            var result = await _repository.GetDefaultCalenderByServiceProviderAsync(serviceProviderDetails.Id);
            return _mapper.Map<IEnumerable<DefaultCalendar>, IEnumerable<DefaultCalendarResource>>(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var defaultCalendar = await _repository.GetdefaultCalendarAsync(id);
            if (defaultCalendar == null)
                return NotFound();

            _repository.Delete(defaultCalendar);

            await _unitOfWork.CompleteAsync();
            return Ok(id);

        }

        private async Task<string> GetUserId()
        {
            var user = await _manager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }
    }
}