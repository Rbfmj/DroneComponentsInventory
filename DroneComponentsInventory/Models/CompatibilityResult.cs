namespace DroneComponentsInventory.Models
{
    public class CompatibilityResult
    {
        public int CheckNumber { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = "unchecked";
        public string Message { get; set; } = null!;
        public int Priority { get; set; }
    }
}
