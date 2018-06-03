using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Knarr.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }    

        public string Address { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePictureFileName { get; set; }

        public ICollection<ServiceProvider> ServiceProviders { get; set; }

        public ApplicationUser()
        {
            ServiceProviders = new Collection<ServiceProvider>();
        }

    }
}