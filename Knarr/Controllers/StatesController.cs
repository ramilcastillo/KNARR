using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Knarr.Controllers.Resources.States;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StatesController : Controller
    {
        private readonly IStateRepository _repository;
        private readonly IMapper _mapper;

        public StatesController(IStateRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<StatesResources>> Get()
        {
            var result = await _repository.GetAllStatesAsync();
            return _mapper.Map<IEnumerable<States>, IEnumerable<StatesResources>>(result);
        }
    }
}