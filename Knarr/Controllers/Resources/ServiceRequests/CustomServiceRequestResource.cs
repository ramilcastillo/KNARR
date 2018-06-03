using Knarr.Core.Models;
using System;
using System.Collections.Generic;

namespace Knarr.Controllers.Resources.ServiceRequests
{
    public class CustomServiceRequestResource
    {
        public int Id { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool IsPending { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int ServiceId { get; set; }

        public string PassengerId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Service Service { get; set; }

        public TimeSpan? TotalTime { get; set; }

        public bool OnRide { get; set; }

        public DateTime? CheckInStartTime { get; set; }

        public decimal AvgServiceReview { get; set; }

        public List<string> ServiceReviewComments { get; set; }

        public decimal AvgClientReview { get; set; }

        public List<string> ClientReviewComments { get; set; }
    }
}
