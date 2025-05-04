using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using System;

namespace ProjectManagement.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarCategory> CarCategories { get; set; }
        public DbSet<CarFeature> CarFeatures { get; set; }
        public DbSet<ProjectManagement.Domain.Entities.CarDriveType> DriveTypes { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RentalApplication> RentalApplications { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(c => c.PricePerDay)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<Brand>()
                   .HasMany(b => b.Cars) 
                   .WithOne(c => c.Brand) 
                   .HasForeignKey(c => c.BrandId); 

                entity.HasOne(c => c.BodyType)
                    .WithMany(bt => bt.Cars)
                    .HasForeignKey(c => c.BodyTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.DriveType)
                  .WithMany(dt => dt.Cars)
                  .HasForeignKey(c => c.DriveTypeId);

                entity.HasOne(c => c.FuelType)
                    .WithMany(ft => ft.Cars)
                    .HasForeignKey(c => c.FuelTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Category)
                    .WithMany(cc => cc.Cars)
                    .HasForeignKey(c => c.CarCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.Features)
                    .WithMany(f => f.Cars)
                    .UsingEntity<Dictionary<string, object>>(
                        "CarCarFeature",
                        j => j.HasOne<CarFeature>().WithMany().HasForeignKey("CarFeatureId"),
                        j => j.HasOne<Car>().WithMany().HasForeignKey("CarId"),
                        j => j.HasKey("CarFeatureId", "CarId")
                    );
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasIndex(b => b.Name).IsUnique();
                entity.Property(b => b.Name).HasMaxLength(50);
                entity.Property(b => b.Country).HasMaxLength(100);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasOne(r => r.Order)
                    .WithMany()
                    .HasForeignKey(r => r.OrderId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(o => o.Car)
                    .WithMany()
                    .HasForeignKey(o => o.CarId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasIndex(p => p.TransactionId).IsUnique();
                entity.Property(p => p.Amount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<RentalApplication>(entity =>
            {
                entity.HasOne(ra => ra.User)
                    .WithMany()
                    .HasForeignKey(ra => ra.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ra => ra.Car)
                    .WithMany()
                    .HasForeignKey(ra => ra.CarId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RentalApplication>()
                .Property(r => r.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasPrecision(18, 2);

            //Дополнительные индексы
            modelBuilder.Entity<CarFeature>().HasIndex(cf => cf.Name).IsUnique();
            modelBuilder.Entity<FuelType>().HasIndex(ft => ft.Name).IsUnique();
            modelBuilder.Entity<BodyType>().HasIndex(bt => bt.Name).IsUnique();
        }
    }
}