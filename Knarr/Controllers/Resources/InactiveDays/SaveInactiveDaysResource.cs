using System;

namespace Knarr.Controllers.Resources.InactiveDays
{
    public class SaveInactiveDaysResource
    {
        public int Id { get; set; }

        public DateTime InactiveDate { get; set; }

        public int ServiceId { get; set; }
    }
}
