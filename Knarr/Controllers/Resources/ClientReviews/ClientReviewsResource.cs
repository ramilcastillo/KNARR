using Knarr.Controllers.Resources.ServiceProviders;
using Knarr.Core.Models;

namespace Knarr.Controllers.Resources.ClientReviews
{
    public class ClientReviewsRerource
    {
        public int Id { get; set; }

        public int RatingPoint { get; set; }

        public string Comments { get; set; }

        public ApplicationUser Passenger { get; set; }

        public ServiceProviderResource ServiceProvider { get; set; }
    }
}
