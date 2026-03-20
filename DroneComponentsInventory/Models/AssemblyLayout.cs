using System.ComponentModel.DataAnnotations;

namespace DroneComponentsInventory.Models
{
    public class AssemblyLayout
    {
        [Key]
        public int BuildId { get; set; }

        [Required]
        public string LayoutJson { get; set; } = string.Empty;

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;

        public DroneBuild? Build { get; set; }
    }
}
