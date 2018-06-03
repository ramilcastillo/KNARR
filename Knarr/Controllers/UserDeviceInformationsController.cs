using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.DeviceInformation;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserDeviceInformationsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserDeviceInformationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserDeviceInformationsController(IMapper mapper, IUserDeviceInformationRepository repository, IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateUserDeviceInformation([FromBody] SaveUserDeviceInformationResources resource)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                resource.UserId = await GetUserId();
                var deviceInformationList = await _repository.GetUserDeviceInformationsAsync(resource.UserId);
                var status = true;
                var userDeviceInformationses =
                    deviceInformationList as UserDeviceInformations[] ?? deviceInformationList.ToArray();
                if (userDeviceInformationses.Any())
                {
                    if (userDeviceInformationses.Any(deviceInfo =>
                        deviceInfo.UserId == resource.UserId && deviceInfo.DeviceId == resource.DeviceId))
                    {
                        status = false;
                    }
                }
                if (status)
                {
                    var details = _mapper.Map<SaveUserDeviceInformationResources, UserDeviceInformations>(resource);
                    _repository.Add(details);
                    await _unitOfWork.CompleteAsync();
                    var saveUserDeviceInformationResources = _mapper.Map(details, resource);
                    result = Ok(saveUserDeviceInformationResources);
                }
                else
                {
                    result = Ok("Information previously stored. No need to store anymore.");
                }
            }
            else
            {
                result = BadRequest(ModelState);
            }

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]SaveUserDeviceInformationResources deviceInformation)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                deviceInformation.UserId = await GetUserId();
                var deviceInformationList = await _repository.GetUserDeviceInformationsAsync(deviceInformation.UserId);

                var deviceDetails = new UserDeviceInformations();

                var userDeviceInformationses =
                    deviceInformationList as UserDeviceInformations[] ?? deviceInformationList.ToArray();

                foreach (var data in userDeviceInformationses)
                {
                    if (data.UserId == deviceInformation.UserId && data.DeviceId == deviceInformation.DeviceId)
                    {
                        deviceDetails = data;
                        break;
                    }
                }

                deviceDetails.FcmToken = deviceInformation.FcmToken;
                await _unitOfWork.CompleteAsync();
                result = Ok(deviceDetails);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }
    }
}