using AutoMapper;
using Knarr.Controllers.Resources.Search;
using Knarr.Controllers.Resources.Services;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Knarr.Controllers.Resources.DistressedUsers;
using Microsoft.AspNetCore.Identity;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class MapController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceRepository _repository;
        private readonly IServiceLocationRepository _locationRepository;
        private readonly IServiceReviewRepository _serviceReviewRepository;
        private readonly IDistressedUsersRepository _distressedUsersRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MapController(IMapper mapper, IServiceRepository repository, 
            IServiceLocationRepository locationRepository, IServiceReviewRepository serviceReviewRepository,
            IServiceProviderRepository serviceProviderRepository,
            IDistressedUsersRepository distressedUsersRepository,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _locationRepository = locationRepository;
            _serviceReviewRepository = serviceReviewRepository;
            _distressedUsersRepository = distressedUsersRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [ActionName("Services")]
        public async Task<IActionResult> Get(decimal? longitude, decimal? latitude)
        {
            IActionResult responseObject;
            if (longitude == null || latitude == null)
            {
                responseObject = BadRequest("Please correct the API call");
            }
            else
            {
                var search = new SearchServiceResources();
                search.Longitude = longitude;
                search.Latitude = latitude;
                var result = _mapper.Map<SearchServiceResources, SearchService>(search);

                responseObject = await GetAllNearestServices(result);
            }
            return responseObject;
        }

        private async Task<IActionResult> GetAllNearestServices(SearchService result)
        {
            var availableServices = await _repository.GetAllNearestAvailableServicesAsyc(result);

            var searchedSericeResult = new List<ServiceSearchResults>();

            foreach (var data in availableServices)
            {
                decimal totalReviewPoint = 0;
                var counter = 0;
                var serviceResult = new ServiceSearchResults();
                var location = await _locationRepository.GetServiceCurrentLocationAsync(data.Id);
                serviceResult.Id = data.Id;
                serviceResult.Title = data.Title;
                serviceResult.RiderPerVessel = data.RiderPerVessel;
                serviceResult.Quantity = data.Quantity;
                serviceResult.IsActive = data.IsActive;
                serviceResult.IsDeleted = data.IsDeleted;
                serviceResult.IsOnRide = data.IsOnRide;
                serviceResult.Rate = data.Rate;
                serviceResult.ServiceProviderId = data.ServiceProviderId;
                serviceResult.StreetAddress = data.StreetAddress;
                serviceResult.City = data.City;
                serviceResult.State = data.State;
                serviceResult.PostalCode = data.PostalCode;
                serviceResult.Longitude = location.Longitude;
                serviceResult.Latitude = location.Latitude;
                serviceResult.ReviewComments = new List<string>();



                var reviewDetails = await _serviceReviewRepository.GetReviews(data.Id);
                var serviceReviews = reviewDetails as ServiceReview[] ?? reviewDetails.ToArray();
                if (Enumerable.Count(serviceReviews) > 0)
                {
                    foreach (var review in serviceReviews)
                    {
                        serviceResult.ReviewComments.Add(review.Comments);
                        totalReviewPoint = totalReviewPoint + review.RatingPoint;
                        counter++;
                    }
                    serviceResult.AvgRatingsPoint = totalReviewPoint / counter;
                }

                searchedSericeResult.Add(serviceResult);
            }

            return Ok(searchedSericeResult);
        }

        private async Task<IActionResult> GetAllDistressedUsers(SearchService result)
        {
            var distressedUsers = await _distressedUsersRepository.GetAllNearestDistressedUsersAsync(result);

            var distressedUsersList = new List<DistressedUsersResource>();
            foreach (var data in distressedUsers)
            {
                var distressedUsersDetails = await _userManager.FindByIdAsync(data.DistressedUserId);
                var details = new DistressedUsersResource();
                details.User = distressedUsersDetails;
                details.Id = data.Id;
                details.DistressedUserId = data.DistressedUserId;
                details.DistressType = data.DistressType;
                details.IsCompleted = data.IsCompleted;
                details.IsConfirmed = data.IsConfirmed;
                details.Longitude = data.Longitude;
                details.Latitude = data.Latitude;

                distressedUsersList.Add(details);
            }

            return Ok(distressedUsersList);
        }
        [HttpGet("{name}")]
        [ActionName("FindServicesByName")]
        public async Task<IEnumerable<ServicesResource>> FindServicesByName(string name)
        {
            var result = await _repository.SerachServiceByNameAsync(name);
            return _mapper.Map<IEnumerable<Service>, IEnumerable<ServicesResource>>(result);
        }
        
        [HttpPost]
        [ActionName("FilterServices")]
        public async Task<IActionResult> FilterServices([FromBody] FilterServices filterServices)
        {
            var result = await _repository.FilterServicesAsync(filterServices);

            var searchResultList = new List<ServiceSearchResults>();

            foreach(var data in result)
            {
                decimal totalReviewPoint = 0;
                var counter = 0;
                var serviceDetails = await _repository.GetServiceAsync(data.Id);
                var serviceLocationDetails = await _locationRepository.GetServiceCurrentLocationAsync(data.Id);

                var searchResult = new ServiceSearchResults();
                searchResult.Id = data.Id;
                searchResult.Title = serviceDetails.Title;
                searchResult.RiderPerVessel = serviceDetails.RiderPerVessel;
                searchResult.Quantity = serviceDetails.Quantity;
                searchResult.IsActive = serviceDetails.IsActive;
                searchResult.IsDeleted = serviceDetails.IsDeleted;
                searchResult.IsOnRide = serviceDetails.IsOnRide;
                searchResult.Rate = data.Rate;
                searchResult.ServiceProviderId = data.ServiceProviderId;
                searchResult.StreetAddress = data.StreetAddress;
                searchResult.City = data.City;
                searchResult.State = data.State;
                searchResult.PostalCode = data.PostalCode;
                searchResult.Longitude = serviceLocationDetails.Longitude;
                searchResult.Latitude = serviceLocationDetails.Latitude;
                searchResult.ReviewComments = new List<string>();

                var reviewDetails = await _serviceReviewRepository.GetReviews(data.Id);

                var serviceReviews = reviewDetails as ServiceReview[] ?? reviewDetails.ToArray();
                if (Enumerable.Count(serviceReviews) > 0)
                {
                    foreach (var review in serviceReviews)
                    {
                        searchResult.ReviewComments.Add(review.Comments);
                        totalReviewPoint = totalReviewPoint + review.RatingPoint;
                        counter++;
                    }
                    searchResult.AvgRatingsPoint = totalReviewPoint / counter;
                }
                

                searchResultList.Add(searchResult);
            }

            return Ok(searchResultList);
        }

        [HttpGet]
        [ActionName("DistressedUsers")]
        public async Task<IActionResult> GetNearestDistressedUsers(decimal? longitude, decimal? latitude)
        {
            IActionResult responseObject;
            if (longitude == null || latitude == null)
            {
                responseObject = BadRequest("Please correct the API call");
            }
            else
            {
                var search = new SearchServiceResources();
                search.Longitude = longitude;
                search.Latitude = latitude;
                var result = _mapper.Map<SearchServiceResources, SearchService>(search);

                responseObject = await GetAllDistressedUsers(result);
            }
            return responseObject;
        }
    }
}