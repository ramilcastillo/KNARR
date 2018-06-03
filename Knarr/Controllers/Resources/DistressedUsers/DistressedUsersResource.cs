using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Knarr.Core.Models;

namespace Knarr.Controllers.Resources.DistressedUsers
{
    public class DistressedUsersResource
    {
        public int Id { get; set; }

        public string DistressedUserId { get; set; }

        public int DistressType { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsCompleted { get; set; }

        public ApplicationUser User { get; set; }
    }
}
