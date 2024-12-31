

using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure;


public class DownTrackContext : DbContext
{
    public DownTrackContext(DbContextOptions options) : base(options) { }

    public DbSet<Technician> Technicians { get; set; }

    public DbSet<Equipment> Equipments { get; set; }

    public DbSet<EquipmentDecommissioning> EquipmentDecommissionings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Technician>().HasIndex(x => x.Id).IsUnique();

        modelBuilder.Entity<Equipment>().HasIndex(x => x.Id).IsUnique();

        modelBuilder.Entity<Equipment>()
       .Property(e => e.Status)
       .HasConversion<int>();

        // Relationship EquipmentDecommissioning - Technician (one-to-many)
        modelBuilder.Entity<EquipmentDecommissioning>()
            .HasOne(b => b.Technician) // EquipmentDecommissioning has a Technician
            .WithMany(t => t.EquipmentDecommissionings) // Technician has many decommissionings
            .HasForeignKey(b => b.TechnicianId) // Foreign key in EquipmentDecommissioning
            .OnDelete(DeleteBehavior.Cascade); // If a Technician is deleted, the decommissioning will be deleted too. 

        // Relationship EquipmentDecommissioning - Equipment (one-to-one)
        modelBuilder.Entity<EquipmentDecommissioning>()
            .HasOne(b => b.Equipment) // EquipmentDecommissioning has an Equipment
            .WithOne(e => e.EquipmentDecommissioning) // Equipment has a single EquipmentDecommissioning
            .HasForeignKey<EquipmentDecommissioning>(b => b.EquipmentId) // Foreign key in EquipmentDecommissioning
            .OnDelete(DeleteBehavior.Cascade); // If an Equipment is deleted, the decommissioning will be deleted too.
    }
}