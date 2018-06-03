using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knarr.Controllers.Resources.ServiceCategories
{
    public class SaveMultipleServiceCategories
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int[] ServiceCategoryId { get; set; }
    }
}
