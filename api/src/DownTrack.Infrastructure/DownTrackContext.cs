

using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure;


public class DownTrackContext : DbContext
{
    public DownTrackContext(DbContextOptions options) : base(options) { }

    public DbSet<Technician> Technicians { get; set; }
    public DbSet<Employee> Employees { get; set; }
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
    }
}