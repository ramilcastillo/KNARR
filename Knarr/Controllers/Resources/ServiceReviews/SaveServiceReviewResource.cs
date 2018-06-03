namespace Knarr.Controllers.Resources.ServiceReviews
{
    public class SaveServiceReviewResource
    {
        public int Id { get; set; }

        public int Serviceid { get; set; }

        public string UserId { get; set; }

        public int RatingPoint { get; set; }

        public string Comments { get; set; }
    }
}
