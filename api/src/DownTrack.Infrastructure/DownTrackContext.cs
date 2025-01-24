

using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DownTrack.Infrastructure;
public class DownTrackContext : IdentityDbContext<User>
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

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

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
                  entity.HasOne<Employee>()
                    .WithOne()
                    .HasForeignKey<Technician>(t => t.Id);

                  entity.Property(t => t.Specialty)
                    .IsRequired();
                  entity.Property(t => t.Salary)
                    .IsRequired();
                  entity.Property(t => t.ExpYears)
                    .IsRequired();

                  entity.HasBaseType<Employee>();
            });

            // Equipment Receptor Region

            modelBuilder.Entity<EquipmentReceptor>(entity =>
            {

                  entity.HasOne<Employee>() // One-to-one relationship with Employee
                    .WithOne()
                    .HasForeignKey<EquipmentReceptor>(t => t.Id);

                  entity.HasBaseType<Employee>();

                  entity.HasOne(er => er.Departament)
                    .WithMany(d => d.EquipmentReceptors)
                    .HasForeignKey(er => er.DepartamentId)
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

                  entity.HasOne(e => e.Location)
                    .WithMany(d => d.Equipments)
                    .HasForeignKey(e => e.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);

                  entity.Property(dm => dm.DateOfadquisition).HasColumnType("date");

            });

            //Transfer Request
            modelBuilder.Entity<TransferRequest>(entity =>
            {
                  entity.HasKey(tr => tr.Id);

                  entity.HasOne(tr => tr.SectionManager)
                    .WithMany(e => e.TransferRequests)
                    .HasForeignKey(tr => tr.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull);

                  entity.HasOne(tr => tr.Equipment)
                    .WithMany(e => e.TransferRequests)
                    .HasForeignKey(tr => tr.EquipmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                  entity.HasOne(tr => tr.ArrivalDepartment)
                    .WithMany(d => d.TransferRequests)
                    .HasForeignKey(tr => tr.ArrivalDepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

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






