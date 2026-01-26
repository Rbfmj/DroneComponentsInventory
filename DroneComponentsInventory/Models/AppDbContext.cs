using Microsoft.EntityFrameworkCore;
using DroneComponentsInventory.Models;

namespace DroneComponentsInventory.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<FrameComponent> FrameComponents { get; set; } = null!;
        public DbSet<MotorsComponent> MotorsComponents { get; set; } = null!;
        public DbSet<FCComponent> FCComponents { get; set; } = null!;   
        public DbSet<ESCComponent> ESCComponents { get; set; } = null!;
        public DbSet<FPVCameraComponent> FPVCameraComponents { get; set; } = null!;
        public DbSet<VideoTransmitterComponent> VideoTransmitterComponent { get; set; } = null!;
        public DbSet<VideoAntennaComponent> VideoAntennaComponents { get; set; } = null!;
        public DbSet<FPVGogglesComponent> FPVGogglesComponent { get; set; } = null!;
        public DbSet<ReceiverComponent> ReceiverComponents { get; set; } = null!;
        public DbSet<ReceiverAntennaComponent> ReceiverAntennaComponents { get; set; } = null!;
        public DbSet<RadioControllerComponent> RadioControllerComponents { get; set; } = null!;
        public DbSet<PropellersComponent> PropellersComponents { get; set; } = null!;
        public DbSet<BatteryComponent> BatteryComponents { get; set; } = null!;
    }

}