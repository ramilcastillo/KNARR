using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Controllers.Resources.DistressedUsers
{
    public class SaveDistressedUsersResource
    {
        public int Id { get; set; }

        public string DistressedUserId { get; set; }

        public int DistressType { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsCompleted { get; set; }
    }
}
