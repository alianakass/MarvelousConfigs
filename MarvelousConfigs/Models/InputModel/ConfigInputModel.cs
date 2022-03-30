using System.ComponentModel.DataAnnotations;

namespace MarvelousConfigs.API.Models
{
    public class ConfigInputModel
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ServiceId { get; set; }
    }
}
