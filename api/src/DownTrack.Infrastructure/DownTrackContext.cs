

using System.Security.Cryptography.X509Certificates;
using DownTrack.Domain.Enitites;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure;


public class DownTrackContext : DbContext
{
    public DownTrackContext(DbContextOptions options) : base(options) { }

    public DbSet<Technician> Technicians { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .ToTable("User")
            .HasKey(u => u.Id);

        modelBuilder.Entity<Technician>()
            .ToTable("Technician")
            .HasOne<User>() // One-to-one relationship with User
            .WithOne()
            .HasForeignKey<Technician>(t => t.Id);
    }
}