using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class SaveAssemblyLayoutRequest
    {
        [Required]
        public int BuildId { get; set; }

        [Required]
        public string LayoutJson { get; set; } = string.Empty;
    }
}
