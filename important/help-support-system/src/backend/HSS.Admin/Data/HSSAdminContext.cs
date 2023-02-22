using HSS.SharedModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HSS.Admin.Data
{
    public class HSSAdminContext : DbContext
    {
        public HSSAdminContext (DbContextOptions<HSSAdminContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TenantEntity>().Property(e => e.Status).HasConversion(new EnumToStringConverter<TenantStatus>());
        }

        public DbSet<TenantEntity> TenantEntity { get; set; } = default!;
    }
}
