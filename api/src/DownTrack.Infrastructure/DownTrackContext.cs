

using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
            .HasOne(er=> er.Departament)
            .WithMany(d=> d.EquipmentReceptors)
            .HasForeignKey(er=> new {er.DepartamentId,er.SectionId})
            .OnDelete(DeleteBehavior.Restrict);


        // Equipment Region
        modelBuilder.Entity<Equipment>()
                    .HasKey(e => e.Id);


        // Maintenance Region
        modelBuilder.Entity<Maintenance>()
                    .HasKey(m => m.Id);


        modelBuilder.Entity<Section>()
            .HasMany(s => s.Departments)
            .WithOne(d => d.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Department>()
            .HasKey(d => new { d.Id, d.SectionId });

        // Evaluation region
        // Technician to Evaluation relationship
        modelBuilder.Entity<Evaluation>()
            .HasOne(e => e.Technician) // Each evaluation has one technician
            .WithMany(t => t.ReceivedEvaluations) // Each technician has many evaluations
            .HasForeignKey(e => e.TechnicianId) // Foreign key in Evaluation
            .OnDelete(DeleteBehavior.Restrict);

        // SectionManager to Evaluation relationship
        modelBuilder.Entity<Evaluation>()
            .HasOne(e => e.SectionManager) // Each evaluation has one section manager
            .WithMany(s => s.GivenEvaluations) // Each section manager has many evaluations
            .HasForeignKey(e => e.SectionManagerId) // Foreign key in Evaluation
            .OnDelete(DeleteBehavior.Restrict);
    }
}


