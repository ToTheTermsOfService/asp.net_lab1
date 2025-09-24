using Lab_1_utilities.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_1_utilities.Data
{
    public class TenantDbContext: DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
        {
        }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<TenantService> TenantServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.ToTable("Tenant");
                entity.Property(t => t.LastName).HasMaxLength(50).IsRequired();
                entity.Property(t => t.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(t => t.MiddleName).HasMaxLength(50);
                entity.Property(t => t.PersonalAccount).HasMaxLength(20).IsRequired();
                entity.HasIndex(t => t.PersonalAccount).IsUnique();
                entity.Property(t => t.Address).HasMaxLength(100).IsRequired();
                entity.Property(t => t.ResidentsCount).IsRequired();
                entity.Property(t => t.ApartmentArea).HasColumnType("decimal(6,2)");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");
                entity.Property(s => s.Name).HasMaxLength(50).IsRequired();
                entity.Property(s => s.BillingType).HasMaxLength(20).IsRequired();
                entity.Property(s => s.Tariff).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<TenantService>(entity =>
            {
                entity.ToTable("Tenant_Service");
                entity.Property(ts => ts.CalculatedAmount).HasColumnType("decimal(10,2)");

                entity.HasOne(ts => ts.Tenant)
                      .WithMany(t => t.TenantServices)
                      .HasForeignKey(ts => ts.TenantId);

                entity.HasOne(ts => ts.Service)
                      .WithMany(s => s.TenantServices)
                      .HasForeignKey(ts => ts.ServiceId);
            });
        }
    }
}
