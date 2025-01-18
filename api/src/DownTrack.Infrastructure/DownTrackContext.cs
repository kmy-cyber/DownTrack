

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

        modelBuilder.Entity<Equipment>().HasIndex(x => x.Id).IsUnique();

        modelBuilder.Entity<Maintenance>().HasIndex(x => x.Id).IsUnique();


        modelBuilder.Entity<Section>()
            .HasMany(s => s.Departments)
            .WithOne(d => d.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Department>()
            .HasKey(d => new { d.Id, d.SectionId });

        modelBuilder.Entity<TransferRequest>()
     .HasOne(tr => tr.Employee)
     .WithMany(e => e.TransferRequests)
     .HasForeignKey(tr => tr.EmployeeId)  // Correcto EmployeeId como FK
     .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TransferRequest>()
        .HasOne(tr => tr.Equipment)
        .WithMany(e => e.TransferRequests)
        .HasForeignKey(tr => tr.EquipmentId)  // Correcto EquipmentId como FK
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TransferRequest>()
            .HasOne(tr => tr.Department)
            .WithMany(d => d.TransferRequests)
            .HasForeignKey(tr => new { tr.DepartmentId, tr.SectionId })  // Correcto DepartmentId como FK
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

