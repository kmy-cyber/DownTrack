

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

    public DbSet<Department> Departments { get; set; }

    public DbSet<TransferRequest> TransferRequests { get; set; }

    public DbSet<Transfer> Transfers { get; set; }

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

        modelBuilder.Entity<Equipment>(entity =>
            {

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(e => e.Location)
                    .WithMany(d => d.Equipments)
                    .HasForeignKey(e => e.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.TransferRequests)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Transfers)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.DateOfadquisition)
                    .IsRequired();
            });

        modelBuilder.Entity<Maintenance>().HasIndex(x => x.Id).IsUnique();


        modelBuilder.Entity<Section>()
            .HasMany(s => s.Departments)
            .WithOne(d => d.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => new { d.Id, d.SectionId });

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Section)
                    .WithMany()
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.TransferRequests)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.Equipments)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        modelBuilder.Entity<TransferRequest>()
            .HasOne(tr => tr.Employee)
            .WithMany(e => e.TransferRequests)
            .HasForeignKey(tr => tr.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TransferRequest>()
            .HasOne(tr => tr.Equipment)
            .WithMany(e => e.TransferRequests)
            .HasForeignKey(tr => tr.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TransferRequest>()
            .HasOne(tr => tr.Department)
            .WithMany(d => d.TransferRequests)
            .HasForeignKey(tr => new { tr.DepartmentId, tr.SectionId })
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasOne(t => t.TransferRequest)
                    .WithMany()
                    .HasForeignKey(t => t.RequestId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(t => t.ShippingSupervisor)
                    .WithMany()
                    .HasForeignKey(t => t.ShippingSupervisorId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(t => t.EquipmentReceptor)
                    .WithMany()
                    .HasForeignKey(t => t.EquipmentReceptorId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.Property(t => t.Date)
                    .IsRequired();
            });
    }
}

