namespace Knarr.Controllers.Resources.ServiceProviderTypes
{
    public class SaveServiceProviderTypeResource
    {
        public int Id { get; set; }

        public int ServiceProviderId { get; set; }

        public int ServiceCategoryId { get; set; }
    }
}
