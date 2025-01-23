

using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DownTrack.Infrastructure
{
    public class DownTrackContext : IdentityDbContext<User>

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {
        public DownTrackContext(DbContextOptions options) : base(options) { }

        public DbSet<Technician> Technicians { get; set; }

        public DbSet<Employee> Employees { get; set; }


        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<DoneMaintenance> DoneMaintenances { get; set; }

        public DbSet<Department> Departments { get; set; }
      
        public DbSet<TransferRequest> TransferRequests { get; set; }

        public DbSet<Transfer> Transfers { get; set; }
        
      public DbSet<Evaluation> Evaluations { get; set; }
      
      public DbSet<EquipmentReceptor> EquipmentReceptors { get; set; }

     


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee Region
            modelBuilder.Entity<Employee>()
                .ToTable("Employee")
                .HasKey(u => u.Id);

        modelBuilder.Entity<Section>()
            .HasMany(s => s.Departments)
            .WithOne(d => d.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        
  
  
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
            // modelBuilder.Entity<Department>()
            //     .Property(d => d.Id)
            //     .ValueGeneratedOnAdd(); //

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Section)
                .WithMany(s => s.Departments)
                .HasForeignKey(d => d.SectionId);


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

