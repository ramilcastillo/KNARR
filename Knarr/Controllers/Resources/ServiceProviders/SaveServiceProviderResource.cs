namespace Knarr.Controllers.Resources.ServiceProviders
{
    public class SaveServiceProviderResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string Information2 { get; set; }

        public string ApplicationUserId { get; set; }

        public bool? IsAvailable { get; set; }

        public int ServiceProviderRequestsStatusId { get; set; }

    }
}
