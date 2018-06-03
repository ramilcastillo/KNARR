using Knarr.Controllers.Resources.Services;
using System;

namespace Knarr.Controllers.Resources.InactiveDays
{
    public class InactiveDaysResource
    {
        public int Id { get; set; }

        public DateTime InactiveDate { get; set; }

        public ServicesResource Services { get; set; }
    }
}
