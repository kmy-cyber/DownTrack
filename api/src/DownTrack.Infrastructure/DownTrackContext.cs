

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

    public DbSet<Evaluation> Evaluations { get; set; }

    public DbSet<EquipmentReceptor> EquipmentReceptors { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employee Region
        modelBuilder.Entity<Employee>()
            .ToTable("Employee")
            .HasKey(u => u.Id);


        // Technician Region
        modelBuilder.Entity<Technician>()
            .ToTable("Technician")
            .HasOne<Employee>() // One-to-one relationship with Employee
            .WithOne()
            .HasForeignKey<Technician>(t => t.Id);

        modelBuilder.Entity<Technician>()
                    .HasBaseType<Employee>();

        // Equipment Receptor Region

        modelBuilder.Entity<EquipmentReceptor>()
            .ToTable("EquipmentReceptor")
            .HasOne<Employee>() // One-to-one relationship with Employee
            .WithOne()
            .HasForeignKey<EquipmentReceptor>(t => t.Id);

        modelBuilder.Entity<EquipmentReceptor>()
                    .HasBaseType<Employee>();

        modelBuilder.Entity<EquipmentReceptor>()
            .HasOne(er => er.Departament)
            .WithMany(d => d.EquipmentReceptors)
            .HasForeignKey(er => new { er.DepartamentId })
            .OnDelete(DeleteBehavior.Restrict);


        // Equipment Region
        modelBuilder.Entity<Equipment>()
                    .HasKey(e => e.Id);

        //Section Region
        modelBuilder.Entity<Section>()
            .HasMany(s => s.Departments)
            .WithOne(d => d.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Section>()
            .HasIndex(s => s.Name)
            .IsUnique();

        //Department Region
        modelBuilder.Entity<Department>()
            .HasKey(d => d.Id); // 

        modelBuilder.Entity<Department>()
            .Property(d => d.Id)
            .ValueGeneratedOnAdd(); //


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

        // Evaluation region
        // Technician to Evaluation relationship
        modelBuilder.Entity<Evaluation>()
            .HasOne(e => e.Technician) // Each evaluation has one technician
            .WithMany(t => t.ReceivedEvaluations) // Each technician has many evaluations
            .HasForeignKey(e => e.TechnicianId) // Foreign key in Evaluation
            .OnDelete(DeleteBehavior.Cascade);

        // SectionManager to Evaluation relationship
        modelBuilder.Entity<Evaluation>()
            .HasOne(e => e.SectionManager) // Each evaluation has one section manager
            .WithMany(s => s.GivenEvaluations) // Each section manager has many evaluations
            .HasForeignKey(e => e.SectionManagerId) // Foreign key in Evaluation
            .OnDelete(DeleteBehavior.SetNull);

    }
}


