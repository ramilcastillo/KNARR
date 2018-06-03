using Knarr.Controllers.Resources.ServiceProviders;
using System;

namespace Knarr.Controllers.Resources.DefaultCalendars
{
    public class DefaultCalendarResource
    {
        public int Id { get; set; }

        public DateTime DefaultDates { get; set; }

        public ServiceProviderResource ServiceProvider { get; set; }
    }
}
