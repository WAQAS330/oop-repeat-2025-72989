using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoCraft.Domain

{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Admin> Admins { get; set; } = null!;

        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<Mechanic> Mechanics { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //You can add your connection string here if needed
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AutoMobileShop;Username=postgres;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // we are configuring the Customer entity here
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            // we are configuring the Car entity here
            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.LicensePlateNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                // Relationships
                entity.HasOne(c => c.VehicleOwner)
                      .WithMany(cu => cu.RegisteredVehicles)
                      .HasForeignKey(c => c.VehicleOwnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // we are configuring the Mechanic entity here
            modelBuilder.Entity<Mechanic>(entity =>
            {
                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            // we are configuring the Service entity here
            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.WorkDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                // These are the relationships between the Service and the Car and the Mechanic
                entity.HasOne(s => s.Vehicle)
                      .WithMany(c => c.MaintenanceServices)
                      .HasForeignKey(s => s.VehicleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Mechanic)
                      .WithMany(m => m.Services)
                      .HasForeignKey(s => s.MechanicId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
