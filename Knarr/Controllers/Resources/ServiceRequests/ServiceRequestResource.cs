using Knarr.Controllers.Resources.Services;
using System;
using Knarr.Core.Models;

namespace Knarr.Controllers.Resources.ServiceRequests
{
    public class ServiceRequestResource
    {
        public int Id { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool IsPending { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeleteDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ServicesResource Services { get; set; }

        public TimeSpan? TotalTime { get; set; }

        public bool OnRide { get; set; }

        public DateTime? CheckInStartTime { get; set; }
    }
}
