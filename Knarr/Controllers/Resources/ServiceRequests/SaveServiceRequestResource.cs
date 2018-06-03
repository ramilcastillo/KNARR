using System;

namespace Knarr.Controllers.Resources.ServiceRequests
{
    public class SaveServiceRequestResource
    {
        public int Id { get; set; }

        public string PassengerId { get; set; }

        public int ServiceId { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool IsPending { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public TimeSpan? TotalTime { get; set; }

        public bool OnRide { get; set; }

        public DateTime? CheckInStartTime { get; set; }
    }
}
