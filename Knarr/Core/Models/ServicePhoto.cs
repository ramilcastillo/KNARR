using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knarr.Core.Models
{
    public class ServicePhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Url { get; set; }

        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
    }
}
