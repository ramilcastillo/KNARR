using Knarr.Controllers.Resources.Services;
using Knarr.Core.Models;

namespace Knarr.Controllers.Resources.ServiceReviews
{
    public class ServiceReviewResource
    {
        public int Id { get; set; }

        public int RatingPoint { get; set; }

        public string Comments { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ServicesResource Service { get; set; }
    }
}
