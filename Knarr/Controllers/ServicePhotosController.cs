using AutoMapper;
using Knarr.Controllers.Resources.ServicesPhotos;
using Knarr.Core;
using Knarr.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knarr.Core.Models.AppSettings;
using Knarr.ServiceClient;

namespace Knarr.Controllers
{
    [Authorize]
    [Route("/api/{serviceId}/photos")]
    public class ServicePhotosController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PhotoAppSettings _photoAppSettings;
        private readonly IServicePhotoRepository _repository;
        private readonly IAwsServiceClient _awsServiceClient;
        private readonly AwsAppSettings _awsAppSettings;

        public ServicePhotosController(IMapper mapper,
            IServiceRepository serviceRepository,
            IUnitOfWork unitOfWork,
             IOptions<PhotoAppSettings> photoSettings,
             IServicePhotoRepository photoRepository,
            IAwsServiceClient awsServiceClient,
            IOptions<AwsAppSettings> awsAppSettings)
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
            _photoAppSettings = photoSettings.Value;
            _repository = photoRepository;
            _awsServiceClient = awsServiceClient;
            _awsAppSettings = awsAppSettings.Value;
        }

        [HttpGet]
        public async Task<IEnumerable<ServicePhotoResource>> GetPhotos(int serviceId)
        {
            var photos = await _repository.GetPhotos(serviceId);

            return _mapper.Map<IEnumerable<ServicePhoto>, IEnumerable<ServicePhotoResource>>(photos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _repository.GetPhoto(id);
            if (photo == null)
                return NotFound();
            _repository.Delete(photo);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int serviceId, List<IFormFile> files)
        {
            try
            {
                var servicePhoto = await _serviceRepository.GetServiceAsync(serviceId);
                if (servicePhoto == null)
                    return NotFound();

                if (files != null && files.Count == 0)
                    return BadRequest("Null file");

                var photos = await _repository.GetPhotos(serviceId);
                var numberOfphotos = 0;
                foreach (var ph in photos)
                {
                    numberOfphotos++;
                }

                if (numberOfphotos >= 8)
                    return BadRequest("Number of allowed photos limited to 8.");

                var results = new List<ServicePhotoResource>();

                if (files != null)
                    foreach (var file in files)
                    {
                        if (file.Length == 0)
                            return BadRequest("Empty file");

                        if (file.Length > _photoAppSettings.MaxBytes)
                            return BadRequest("Maximum file size exceeded");

                        if (!_photoAppSettings.IsSupported(file.FileName))
                            return BadRequest("Invalid file type");

                        var awsServiceSettings = new AwsServiceClientSettings(file, _awsAppSettings.BucketName,
                            _awsAppSettings.SubFolderService, _awsAppSettings.BucketLocation,
                            _awsAppSettings.PublicDomain);

                        var photoUrl = await _awsServiceClient.UploadAsync(awsServiceSettings);

                        var photo = new ServicePhoto {Url = photoUrl};
                        servicePhoto.Photos.Add(photo);

                        await _unitOfWork.CompleteAsync();

                        results.Add(_mapper.Map<ServicePhoto, ServicePhotoResource>(photo));
                    }

                if (results.Count > 0)
                {
                    return Ok(results);
                }
                else
                {
                    return BadRequest("Unable to upload");
                }
            }
            catch (Exception ex)
            {
                var message = $"Message: {ex.Message}\n Source:{ex.Source} \n Expection:{ex.InnerException}";
                return BadRequest(message);
            }
        }
    }
}
