using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using EStore.IdentityServer.Models;
using EStore.Common.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EStore.IdentityServer.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {


    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderEntity>().Property(e => e.Status).HasConversion(new EnumToStringConverter<OrderStatus>());
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{FriendEntity}"/>.
    /// </summary>
    public DbSet<ProductEntity> Products { get; set; }

    public DbSet<CartEntity> Carts { get; set; }

    public DbSet<CartItemEntity> CartItems { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<OrderItemEntity> OrderItems { get; set; }
}
