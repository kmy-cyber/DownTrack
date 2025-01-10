

using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure;


public class DownTrackContext : IdentityDbContext<User>

{
    public DownTrackContext(DbContextOptions options) : base(options) { }

    public DbSet<Technician> Technicians { get; set; }

    public DbSet<Employee> Employees { get; set; }


    public DbSet<Equipment> Equipments { get; set; }

    public DbSet<Section> Sections { get; set; }

    public DbSet<DoneMaintenance> DoneMaintenances { get; set; }

    public DbSet<Department> Departments { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        #region Employee
        modelBuilder.Entity<Employee>()
            .ToTable("Employee")
            .HasKey(u => u.Id);
        #endregion

        #region Technician
        modelBuilder.Entity<Technician>()
            .ToTable("Technician")
            .HasOne<Employee>() // One-to-one relationship with Employee
            .WithOne()
            .HasForeignKey<Technician>(t => t.Id);
        #endregion

        modelBuilder.Entity<Technician>()
            .HasBaseType<Employee>();

        // modelBuilder.Entity<User>()
        //     .HasOne<Employee>()
        //     .WithOne()
        //     .HasForeignKey<User>(u=> u.IdEmployee)
        //     .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Equipment>().HasIndex(x => x.Id).IsUnique();

        modelBuilder.Entity<Section>()
            .HasMany(s => s.Departments)
            .WithOne(d => d.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Department>()
            .HasKey(d => new { d.Id, d.SectionId });

        // Configuration of DoneMaintenance relationship

        modelBuilder.Entity<DoneMaintenance>()
            .HasKey(mr => mr.Id); // Primary key of the relationship

        modelBuilder.Entity<DoneMaintenance>()
            .HasOne(dm => dm.Technician) // Primary key of the relationship
            .WithMany(t => t.DoneMaintenances) // One-to-many relationship
            .HasForeignKey(dm => dm.TechnicianId) // Foreign key to TechnicianId
            .OnDelete(DeleteBehavior.SetNull); // If a technician is deleted, set the value of the field to null

        modelBuilder.Entity<DoneMaintenance>()
            .HasOne(dm => dm.Equipment)
            .WithMany(e => e.DoneMaintenances)
            .HasForeignKey(dm => dm.EquipmentId)
            .OnDelete(DeleteBehavior.SetNull); // If an equipment is deleted, set the value of the field to null

        modelBuilder.Entity<DoneMaintenance>()
            .Property(dm => dm.Date).HasColumnType("date");

    }
}


