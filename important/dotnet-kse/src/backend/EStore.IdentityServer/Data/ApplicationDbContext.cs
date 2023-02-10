using Duende.IdentityServer.EntityFramework.Options;
using EStore.SharedModels.Entities;
using EStore.IdentityServer.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;

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
