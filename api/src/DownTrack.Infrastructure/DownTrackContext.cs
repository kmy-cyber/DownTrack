

using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DownTrack.Infrastructure;
public class DownTrackContext : IdentityDbContext<User,Role,int>
{

  public DownTrackContext(DbContextOptions options) : base(options)
  {

  }

  public DbSet<Technician> Technicians { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Equipment> Equipments { get; set; }
  public DbSet<Section> Sections { get; set; }
  public DbSet<DoneMaintenance> DoneMaintenances { get; set; }
  public DbSet<Department> Departments { get; set; }
  public DbSet<Evaluation> Evaluations { get; set; }
  public DbSet<EquipmentReceptor> EquipmentReceptors { get; set; }
  public DbSet<TransferRequest> TransferRequests { get; set; }
  public DbSet<Transfer> Transfers { get; set; }
  public DbSet<EquipmentDecommissioning> EquipmentDecommissionings { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>(entity=>
    {
      entity.HasKey(e=> e.Id);
    });

    modelBuilder.Entity<Role>(entity=>
    {
      entity.HasKey(e=> e.Id);
    });
    
    // Employee Region
    modelBuilder.Entity<Employee>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.Name)
                .IsRequired();
      entity.Property(e => e.UserRole)
                .IsRequired();

      entity.HasIndex(e => e.Name);
      entity.HasIndex(e => e.UserRole);

    });


    // Technician Region
    modelBuilder.Entity<Technician>(entity =>
    {
      entity.ToTable("Technicians");

      entity.HasBaseType<Employee>();

      entity.HasOne<Employee>()
                .WithOne()
                .HasForeignKey<Technician>(t => t.Id);

      entity.Property(t => t.Specialty)
                .IsRequired();
      entity.Property(t => t.Salary)
                .IsRequired();
      entity.Property(t => t.ExpYears)
                .IsRequired();


    });

    // Equipment Receptor Region

    modelBuilder.Entity<EquipmentReceptor>(entity =>
    {
      entity.ToTable("EquipmentReceptors");

      entity.HasBaseType<Employee>();

      entity.HasOne<Employee>() // One-to-one relationship with Employee
                .WithOne()
                .HasForeignKey<EquipmentReceptor>(t => t.Id);

      entity.HasOne(er => er.Department)
                .WithMany(d => d.EquipmentReceptors)
                .HasForeignKey(er => er.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

    });

    //Section region

    modelBuilder.Entity<Section>(entity =>
    {
      entity.HasKey(s => s.Id);

      entity.Property(s => s.Name)
                .IsRequired();
      entity.HasIndex(s => s.Name)
                .IsUnique();
      entity.Property(s=>s.CreatedDate)
            .IsRequired()
            .HasColumnType("date");

      entity.HasOne(s => s.SectionManager)
                .WithMany(sm => sm.Sections)
                .HasForeignKey(s => s.SectionManagerId)
                .OnDelete(DeleteBehavior.Restrict);


    });


    //Department Region

    modelBuilder.Entity<Department>(entity =>
    {
      entity.HasKey(d => d.Id);

      entity.Property(d => d.Name)
              .IsRequired()
              .HasMaxLength(100);

      entity.HasIndex(d => d.Name);
      entity.HasIndex(d => d.SectionId);
      
      entity.Property(d=>d.CreatedDate)
            .IsRequired()
            .HasColumnType("date");
            
      entity.HasOne(d => d.Section)
              .WithMany(s => s.Departments)
              .HasForeignKey(d => d.SectionId)
              .OnDelete(DeleteBehavior.Cascade);

    });

    // Equipment Region
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

      entity.Property(e => e.DateOfadquisition)
              .IsRequired();

      entity.HasIndex(e => e.Type);
      entity.HasIndex(e => e.Name);
      entity.HasIndex(e => e.DateOfadquisition);
      entity.HasIndex(e => e.Status);

      entity.HasOne(e => e.Department)
                .WithMany(d => d.Equipments)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

      entity.Property(dm => dm.DateOfadquisition).HasColumnType("date");

    });

    //Transfer Request
    modelBuilder.Entity<TransferRequest>(entity =>
    {
      entity.HasKey(tr => tr.Id);

      entity.Property(tr=>tr.Status)
            .IsRequired();
      
      entity.Property(tr=> tr.Date)
            .HasColumnType("date")
            .IsRequired();

      entity.HasOne(tr => tr.SectionManager)
                .WithMany(e => e.TransferRequests)
                .HasForeignKey(tr => tr.SectionManagerId)
                .OnDelete(DeleteBehavior.SetNull);

      entity.HasOne(tr=> tr.SourceDepartment)
            .WithMany(d=> d.IncomingTransferRequests)
            .HasForeignKey(tr=> tr.SourceDepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

      entity.HasOne(tr => tr.Equipment)
                .WithMany(e => e.TransferRequests)
                .HasForeignKey(tr => tr.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(tr => tr.ArrivalDepartment)
                .WithMany(d => d.OutgoingTransferRequests)
                .HasForeignKey(tr => tr.ArrivalDepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

      // entity.Property(tr=> tr.Status)
      //       .HasDefaultValue("Unregistered");       

    });

    // Equipment decommissioning region

    modelBuilder.Entity<EquipmentDecommissioning>(entity =>
    {

      entity.HasKey(ed => ed.Id);

      entity.Property(ed => ed.Date)
            .HasColumnType("date")
            .IsRequired();

      entity.Property(ed => ed.Cause)
            .IsRequired();

      entity.Property(ed => ed.Status)
            .IsRequired();

      entity.HasIndex(ed => ed.Date);
      entity.HasIndex(ed => ed.Status);

      // EquipmentDecommissioning - Technician relationship (one-to-many)

      entity.HasOne(ed => ed.Technician) // EquipmentDecommissioning has one Technician
            .WithMany(t => t.EquipmentDecommissionings) // Technician has many EquipmentDecommissionings
            .HasForeignKey(ed => ed.TechnicianId) // Foreign key in EquipmentDecommissioning
            .OnDelete(DeleteBehavior.SetNull); // If Technician is deleted, set TechnicianId to null

      // EquipmentDecommissioning - Equipment relationship (one-to-many)

      entity.HasOne(ed => ed.Equipment) // 
            .WithMany(e => e.EquipmentDecommissionings) //
            .HasForeignKey(ed => ed.EquipmentId) // 
            .OnDelete(DeleteBehavior.SetNull); // 

      // EquipmentDecommissioning - Receptor relationship (one-to-many)

      entity.HasOne(ed => ed.Receptor) // EquipmentDecommissioning has one Receptor
            .WithMany(r => r.EquipmentDecommissionings) // Receptor has many EquipmentDecommissionings
            .HasForeignKey(ed => ed.ReceptorId) // Foreign key in EquipmentDecommissioning
            .OnDelete(DeleteBehavior.SetNull); // If Receptor is deleted, set ReceptorId to null

    });

    // Configuration of DoneMaintenance relationship

    modelBuilder.Entity<DoneMaintenance>(entity =>
    {
      entity.HasKey(dm => dm.Id); // Primary key of the relationship

      entity.Property(dm => dm.Type)
                .IsRequired();

      entity.Property(dm => dm.Date)
                .HasColumnType("date")
                .IsRequired();

      entity.Property(dm => dm.Cost)
                .IsRequired();


      entity.HasIndex(dm => dm.Date);

      entity.HasOne(dm => dm.Technician) // Primary key of the relationship
                .WithMany(t => t.DoneMaintenances) // One-to-many relationship
                .HasForeignKey(dm => dm.TechnicianId) // Foreign key to TechnicianId
                .OnDelete(DeleteBehavior.SetNull); // If a technician is deleted, set the value of the field to null


      entity.HasOne(dm => dm.Equipment)
                .WithMany(e => e.DoneMaintenances)
                .HasForeignKey(dm => dm.EquipmentId)
                .OnDelete(DeleteBehavior.SetNull); // If an equipment is deleted, set the value of the field to null


    });


    // Evaluation region
    // Technician to Evaluation relationship
    modelBuilder.Entity<Evaluation>(entity =>
    {
      entity.HasKey(ev => ev.Id);

      entity.Property(ev => ev.Description)
                .IsRequired();

      entity.HasOne(e => e.Technician) // Each evaluation has one technician
                .WithMany(t => t.ReceivedEvaluations) // Each technician has many evaluations
                .HasForeignKey(e => e.TechnicianId) // Foreign key in Evaluation
                .OnDelete(DeleteBehavior.Cascade);

      // SectionManager to Evaluation relationship
      entity.HasOne(e => e.SectionManager) // Each evaluation has one section manager
                .WithMany(s => s.GivenEvaluations) // Each section manager has many evaluations
                .HasForeignKey(e => e.SectionManagerId) // Foreign key in Evaluation
                .OnDelete(DeleteBehavior.SetNull);

    });

    //Transfer Region
    modelBuilder.Entity<Transfer>(entity =>
      {
        entity.HasKey(t => t.Id);

        entity.Property(t => t.Date)
              .HasColumnType("date")
              .IsRequired();

        entity.HasIndex(t => t.Date);

        entity.HasOne(t => t.TransferRequest)
              .WithOne(tr => tr.Transfer)
              .HasForeignKey<Transfer>(t => t.RequestId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(t => t.ShippingSupervisor)
              .WithMany(sr => sr.Transfers)
              .HasForeignKey(t => t.ShippingSupervisorId)
              .OnDelete(DeleteBehavior.SetNull);

        entity.HasOne(t => t.EquipmentReceptor)
              .WithMany(er => er.AcceptedTransfers)
              .HasForeignKey(t => t.EquipmentReceptorId)
              .OnDelete(DeleteBehavior.SetNull);

      });


  }

}






