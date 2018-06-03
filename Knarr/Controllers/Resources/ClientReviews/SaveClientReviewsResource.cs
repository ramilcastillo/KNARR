namespace Knarr.Controllers.Resources.ClientReviews
{
    public class SaveClientReviewsResource
    {
       
        public int Id { get; set; }
        
        public int ServiceProviderId { get; set; } 
            
        public string PassengerId { get; set; }

        public int RatingPoint { get; set; }

        public string Comments { get; set; }
    }
}
