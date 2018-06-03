using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Knarr.Core.Models.AppSettings;
using Knarr.ServiceClient;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ProfilesController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly AwsAppSettings _awsAppSettings;
        private readonly PhotoAppSettings _photoAppSettings;
        private readonly IAwsServiceClient _awsServiceClient;

        public ProfilesController(UserManager<ApplicationUser> manager, IOptions<PhotoAppSettings> photoSettings, IOptions<AwsAppSettings> awsSettings, IServiceProviderRepository serviceProviderRepository, IAwsServiceClient awsServiceClient)
        {
            _manager = manager;
            _awsAppSettings = awsSettings.Value;
            _photoAppSettings = photoSettings.Value;
            _awsServiceClient = awsServiceClient;
        }


        //GET api/Profiles/getProfile
        [HttpGet]
        [ActionName("getProfile")]
        public async Task<ApplicationUser> GetProfile()
        {
            return await _manager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
        }

        //PUT api/Profiles/updateProfile
        [HttpPut]
        [ActionName("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ApplicationUser user)
        {
            var userInformation = await _manager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            if (userInformation == null)
            {
                return NotFound();
            }
            userInformation.FirstName = user.FirstName;
            userInformation.LastName = user.LastName;
            userInformation.PhoneNumber = user.PhoneNumber;
            userInformation.Address = user.Address;
            userInformation.Country = user.Country;
            userInformation.State = user.State;

            var result = await _manager.UpdateAsync(userInformation);
            if (result.Succeeded)
            {
                return Ok(userInformation);
            }
            var sb = new StringBuilder();
            foreach (var error in result.Errors)
            {
                sb.Append(error);
                sb.Append("\n");
            }

            return BadRequest(sb.ToString());
        }

        //PUT api/Profiles/updateProfileImage
        [HttpPut]
        [ActionName("updateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage(IFormFile file)
        {
            try
            {
                var userInformation = await _manager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
                if (userInformation == null)
                {
                    return NotFound();
                }

                if (file != null)
                {
                    if (file.Length == 0)
                    {
                        return BadRequest("Empty File");
                    }

                    if (file.Length > _photoAppSettings.MaxBytes)
                    {
                        return BadRequest("Maximum file size exceeded");
                    }

                    if (!_photoAppSettings.IsSupported(file.FileName))
                    {
                        return BadRequest("Invalid file type");
                    }

                    var awsServiceclientSettings = new AwsServiceClientSettings(file,
                        _awsAppSettings.BucketName, _awsAppSettings.SubFolderProfile, _awsAppSettings.BucketLocation, _awsAppSettings.PublicDomain);

                    var photoUrl = await _awsServiceClient.UploadAsync(awsServiceclientSettings);

                    userInformation.ProfilePictureFileName = photoUrl;

                    if (!string.IsNullOrWhiteSpace(photoUrl))
                    {
                        var result = await _manager.UpdateAsync(userInformation);
                        if (result.Succeeded)
                        {
                            return Ok(userInformation);
                        }
                        var sb = new StringBuilder();
                        foreach (var error in result.Errors)
                        {
                            sb.Append(error);
                            sb.Append("\n");
                        }
                        return BadRequest(sb.ToString());
                    }
                }
                return NotFound("Please select an Image for update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }
    }
}