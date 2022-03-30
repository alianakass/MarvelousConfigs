using System.ComponentModel.DataAnnotations;

namespace MarvelousConfigs.API.Models
{
    public class MicroserviceInputModel
    {
        [Required]
        public string ServiceName { get; set; }
        [Required]
        [Url]
        public string URL { get; set; }
    }
}
