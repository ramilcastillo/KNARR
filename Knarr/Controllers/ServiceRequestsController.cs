using AutoMapper;
using Knarr.Controllers.Resources.ServiceRequests;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Knarr.Controllers.Resources.Notification;
using Knarr.Controllers.Resources.Notification.FireBase;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ServiceRequestsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceProviderRepository _serviceProviderRepository;
        private readonly IServiceReviewRepository _serviceReviewRepository;
        private readonly IClientReviewRepository _clientReviewRepository;
        private readonly IUserDeviceInformationRepository _userDeviceInfoRepository;
        private readonly IFireBaseServiceClient _fireBaseServiceClient;

        public ServiceRequestsController(IMapper mapper, IServiceRequestRepository repository, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IServiceRepository serviceRepository,IServiceProviderRepository serviceProviderRepository
            ,IServiceReviewRepository serviceReviewRepository, IClientReviewRepository clientReviewRepository,IUserDeviceInformationRepository userDeviceInfoRepository,
            IFireBaseServiceClient fireBaseServiceClient)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _serviceRepository = serviceRepository;
            _serviceProviderRepository = serviceProviderRepository;
            _serviceReviewRepository = serviceReviewRepository;
            _clientReviewRepository = clientReviewRepository;
            _userDeviceInfoRepository = userDeviceInfoRepository;
            _fireBaseServiceClient = fireBaseServiceClient;
        }

        //GET api/RideRequests/getServiceRequestDetails/id
        [HttpGet("{id}")]
        [ActionName("getServiceRequestDetails")]
        public async Task<ServiceRequestResource> GetServiceRequestDetails(int id)
        {
            var serviceRequest = await _repository.GetServiceRequestDetails(id);
            return _mapper.Map<ServiceRequest, ServiceRequestResource>(serviceRequest);
        }

        //POST api/RideRequests/createServiceRequest
        [HttpPost]
        [ActionName("createServiceRequest")]
        public async Task<IActionResult> CreateServiceRequest([FromBody] SaveServiceRequestResource serviceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = _mapper.Map<SaveServiceRequestResource, ServiceRequest>(serviceRequest);
            request.PassengerId = await GetUserId();
            request.RequestDate = DateTime.UtcNow;
            _repository.CreateRideRequest(request);
            await _unitOfWork.CompleteAsync();
            var requestResult = _mapper.Map<ServiceRequest, SaveServiceRequestResource>(request);
            return Ok(requestResult);
        }

        //GET api/RideRequests/getAllserviceRequests
        [HttpGet]
        [ActionName("getAllserviceRequests")]
        public async Task<IEnumerable<ServiceRequestResource>> GetAllServiceRequests()
        {
            var userInfo = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);

            var allServiceRequests = await _repository.GetAllServiceRequestsAsync(userInfo.Id);
            return _mapper.Map<IEnumerable<ServiceRequest>, IEnumerable<ServiceRequestResource>>(allServiceRequests);
        }

        [HttpGet]
        [ActionName("getServiceRequestsByServiceProvider")]
        public async Task<List<CustomServiceRequestResource>> GetServiceRequestsByServiceProvider()
        {
            var serviceRequests = new List<CustomServiceRequestResource>();
            var id = await GetServiceProviderId();
            var allServiceRequests = await _repository.GetAllServiceRequestsByServiceproviderAsync(id);
            
            foreach(var serviceReq in allServiceRequests)
            {
                decimal serviceAvgRating = 0;
                var serviceRatingCounter = 0;

                decimal clientAvgRatingpoint = 0;
                var clientRatingCounter = 0;

                var customServiceReq = new CustomServiceRequestResource();
                customServiceReq.Id = serviceReq.Id;
                customServiceReq.RequestDate = serviceReq.RequestDate;
                customServiceReq.IsPending = serviceReq.IsPending;
                customServiceReq.IsApproved = serviceReq.IsApproved;
                customServiceReq.IsDeleted = serviceReq.IsDeleted;
                customServiceReq.DeleteDate = serviceReq.DeletedDate;
                customServiceReq.TotalTime = serviceReq.TotalTime;
                customServiceReq.OnRide = serviceReq.OnRide;
                customServiceReq.CheckInStartTime = serviceReq.CheckInStartTime;
                customServiceReq.ServiceId = serviceReq.ServiceId;
                customServiceReq.PassengerId = serviceReq.PassengerId;
                customServiceReq.Service = serviceReq.Service;
                customServiceReq.ApplicationUser = serviceReq.ApplicationUser;
               
                customServiceReq.ServiceReviewComments = new List<string>();
                var serviceRatingDetails =await _serviceReviewRepository.GetReviews(serviceReq.ServiceId);

                var serviceReviews = serviceRatingDetails as ServiceReview[] ?? serviceRatingDetails.ToArray();
                if(serviceReviews.Any())
                {
                    foreach (var srd in serviceReviews)
                    {
                        customServiceReq.ServiceReviewComments.Add(srd.Comments);
                        serviceAvgRating = serviceAvgRating + srd.RatingPoint;
                        serviceRatingCounter++;
                    }
                    customServiceReq.AvgServiceReview = serviceAvgRating / serviceRatingCounter;
                }
                

                customServiceReq.ClientReviewComments = new List<string>();

                var clientRatingDetails = await _clientReviewRepository.GetClientReviewAsync(serviceReq.PassengerId);

                var clientReviewses = clientRatingDetails as ClientReviews[] ?? clientRatingDetails.ToArray();
                if(clientReviewses.Any())
                {
                    foreach (var crd in clientReviewses)
                    {
                        customServiceReq.ClientReviewComments.Add(crd.Comments);
                        clientAvgRatingpoint = clientAvgRatingpoint + crd.RatingPoint;
                        clientRatingCounter++;
                    }
                    customServiceReq.AvgClientReview = clientAvgRatingpoint / clientRatingCounter;
                }
                serviceRequests.Add(customServiceReq);
                
            }

            return serviceRequests;
        }

        //PUT api/RideRequests/approveServiceRequest/id
        [HttpPut("{id}")]
        [ActionName("approveServiceRequest")]
        public async Task<IActionResult> UpdateServiceRequestForApprove(int id)
        {
            var result = await _repository.GetServiceRequestDetails(id);
            if (result == null)
            {
                return NotFound();
            }

            if (_repository.IsServiceProviderAlreadyOnRide(result.Service.ServiceProviderId))
            {
                return BadRequest("Service Provider is already on ride. At this moment this service provider can not approve ride request");
            }
            
            result.IsPending = false;
            result.IsApproved = true;

            await _unitOfWork.CompleteAsync();

            //Start Sending notification after approve a Service request

            var approvedNotification = new NotificationInformation();
            approvedNotification.DeviceInformations = await _userDeviceInfoRepository.GetUserDeviceInformationsAsync(result.ApplicationUser.Id);
            approvedNotification.NotiTitle = "Knarr";
            approvedNotification.NotiMessage = "Ride Request Approved";

            await _fireBaseServiceClient.SendNotification(approvedNotification);

            //End Sending notification after approve a Service request
            return Ok(result);
        }

        //PUT api/RideRequests/deleteServiceRequest/id
        [HttpPut("{id}")]
        [ActionName("deleteServiceRequest")]
        public async Task<IActionResult> UpdateRideRequestForDelete(int id)
        {
            var result = await _repository.GetServiceRequestDetails(id);
            if (result == null)
            {
                return NotFound();
            }

            if (result.OnRide)
                return BadRequest("Ride already started");

            result.IsPending = false;
            result.IsDeleted = true;
            result.DeletedDate = DateTime.UtcNow;

            await _unitOfWork.CompleteAsync();

            //Start Sending notification after approve a Service request

            var approvedNotification = new NotificationInformation();
            approvedNotification.DeviceInformations = await _userDeviceInfoRepository.GetUserDeviceInformationsAsync(result.ApplicationUser.Id);
            approvedNotification.NotiTitle = "Knarr";
            approvedNotification.NotiMessage = "Ride Request Canceled";

            await _fireBaseServiceClient.SendNotification(approvedNotification);

            //End Sending notification after approve a Service request

            return Ok(id);
        }

        //PUT api/riderequests/checkIn/id
        [HttpPut("{id}")]
        [ActionName("checkIn")]
        public async Task<IActionResult> UpdateOnRide(int id, [FromBody]SaveServiceRequestResource resources)
        {
            var serviceRequest = await _repository.GetServiceRequestDetails(id);

            if (serviceRequest == null)
                return NotFound();

            serviceRequest.IsPending = false;
            serviceRequest.IsApproved = true;
            serviceRequest.OnRide = true;
            serviceRequest.CheckInStartTime = resources.CheckInStartTime;

            await _unitOfWork.CompleteAsync();

            //Update service for this service request as that service is on ride

            var serviceInfo = await _serviceRepository.GetServiceAsync(serviceRequest.ServiceId);
            serviceInfo.IsOnRide = true;

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<ServiceRequest, SaveServiceRequestResource>(serviceRequest);

            return Ok(result);
        }

        //PUT api/riderequests/checkout/id
        [HttpPut("{id}")]
        [ActionName("checkout")]
        public async Task<IActionResult> UpdateTotalTime(int id, [FromBody]SaveServiceRequestResource resources)
        {
            var serviceRequest = await _repository.GetServiceRequestDetails(id);

            if (serviceRequest == null)
                return NotFound();

            serviceRequest.TotalTime = resources.TotalTime;
            serviceRequest.OnRide = false;

            await _unitOfWork.CompleteAsync();

            //after completing ride service complete the ride and status should be changed

            var serviceInfo = await _serviceRepository.GetServiceAsync(serviceRequest.ServiceId);
            serviceInfo.IsOnRide = false;

            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<ServiceRequest, SaveServiceRequestResource>(serviceRequest);

            return Ok(result);
        }

        [HttpGet]
        [ActionName("getOnRideServiceRequestDetailsByUser")]
        public async Task<ServiceRequestResource> GetOnRideServiceRequestDetails()
        {
            var onRideServiceDetails = await _repository.GetOnrideServcieRequestDetailsAsync(await GetUserId());
            return _mapper.Map<ServiceRequest, ServiceRequestResource>(onRideServiceDetails);
        }

        [HttpGet]
        [ActionName("getOnRideServiceRequestDetailsByServiceProvider")]
        public async Task<ServiceRequestResource> GetOnRideServiceRequestDetailsByServiceProvider()
        {
            var onRideServiceDetails = await _repository.GetOnrideServcieRequestDetailsAsyncByServiceProvider(await GetServiceProviderId());
            return _mapper.Map<ServiceRequest, ServiceRequestResource>(onRideServiceDetails);
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            return user.Id;
        }

        private async Task<int> GetServiceProviderId()
        {
            var userId = await GetUserId();
            var serviceProviderDetails = await _serviceProviderRepository.GetServiceProviderDetailsByUserAsync(userId);
            return serviceProviderDetails.Id;
        }
    }
}