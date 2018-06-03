using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.DistressedUsers;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DistressedUsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDistressedUsersRepository _distressedUsersRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public DistressedUsersController(IMapper mapper, IDistressedUsersRepository distressedUsersRepository,
            UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _distressedUsersRepository = distressedUsersRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<SaveDistressedUsersResource> GetDistressedUserDetails(int id)
        {
            var details = await _distressedUsersRepository.GetDistressedRequestDetailsAsync(id);
            return _mapper.Map<DistressedUsers, SaveDistressedUsersResource>(details);
        }

        [HttpGet]
        public async Task<IActionResult> GetDistressedUsers(string filterBy = null)
        {
            IActionResult result;
            try
            {
                if (string.IsNullOrWhiteSpace(filterBy))
                {
                    var user = await GetUserId();
                    var allDistressedRequests = await _distressedUsersRepository.GetDistressedRequestAsync(user);
                    result = Ok(_mapper.Map<IEnumerable<DistressedUsers>,IEnumerable<DistressedUsersResource>>(allDistressedRequests));
                }
                else
                {
                    switch (filterBy.ToLower())
                    {
                        case "own":
                            var ownAllDistressedRequests =
                                await _distressedUsersRepository.GetOwnDistressedRequestsAsync(await GetUserId());
                            result = Ok(
                                _mapper.Map<IEnumerable<DistressedUsers>, IEnumerable<DistressedUsersResource>>(
                                    ownAllDistressedRequests));
                            break;
                        default:
                            result = BadRequest("The specified parameter is not valid");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                result = BadRequest(e.ToString());
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaveDistressedUsersResource resource)
        {
            IActionResult result;
            try
            {
                if (ModelState.IsValid)
                {
                    resource.DistressedUserId = await GetUserId();
                    if (!await _distressedUsersRepository.IsAnyIncompleteDistressedRequestByUserAsync(resource.DistressedUserId))
                    {
                        resource.IsCompleted = false;
                        var distressedInfo = _mapper.Map<SaveDistressedUsersResource, DistressedUsers>(resource);
                        _distressedUsersRepository.Create(distressedInfo);
                        result = Ok();
                    }
                    else
                    {
                        result = BadRequest("Previous Distress request is not completed.");
                    }
                    
                }
                else
                {
                    result = BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                result = BadRequest(e.ToString());
            }
            
            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SaveDistressedUsersResource resource)
        {
            IActionResult result;
            try
            {
                if (ModelState.IsValid)
                {
                    resource.DistressedUserId = await GetUserId();
                    var details = await _distressedUsersRepository.GetDistressedRequestDetailsAsync(id);
                    var mappedInfo = _mapper.Map<SaveDistressedUsersResource, DistressedUsers>(resource, details);
                    await _unitOfWork.CompleteAsync();
                    return Ok();
                }
                else
                {
                    result = BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                result = BadRequest(e.ToString());
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