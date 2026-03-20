using Microsoft.EntityFrameworkCore;

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
        public DbSet<VideoTransmitterComponent> VideoTransmitterComponents { get; set; } = null!;
        public DbSet<VideoAntennaComponent> VideoAntennaComponents { get; set; } = null!;
        public DbSet<FPVGogglesComponent> FPVGogglesComponents { get; set; } = null!;
        public DbSet<ReceiverComponent> ReceiverComponents { get; set; } = null!;
        public DbSet<ReceiverAntennaComponent> ReceiverAntennaComponents { get; set; } = null!;
        public DbSet<RadioControllerComponent> RadioControllerComponents { get; set; } = null!;
        public DbSet<PropellersComponent> PropellersComponents { get; set; } = null!;
        public DbSet<BatteryComponent> BatteryComponents { get; set; } = null!;
        public DbSet<DroneBuild> DroneBuilds { get; set; } = null!;
        public DbSet<AssemblyLayout> AssemblyLayouts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AssemblyLayout>()
                .HasOne(layout => layout.Build)
                .WithOne()
                .HasForeignKey<AssemblyLayout>(layout => layout.BuildId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}