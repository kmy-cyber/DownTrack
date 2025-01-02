

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

    public DbSet<Maintenance> Maintenances { get; set; }

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

        modelBuilder.Entity<Section>().HasIndex(x => x.Id).IsUnique();

        modelBuilder.Entity<Maintenance>().HasIndex(x => x.Id).IsUnique();


    }
}


