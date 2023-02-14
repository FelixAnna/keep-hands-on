using HSS.IdentityServer.Models;
using HSS.SharedModels.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;

namespace HSS.IdentityServer.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TenantEntity>().Property(e => e.Status).HasConversion(new EnumToStringConverter<TenantStatus>());
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{GroupEntity}"/>.
    /// </summary>
    public DbSet<GroupEntity> Groups { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{GroupMemberEntity}"/>.
    /// </summary>
    public DbSet<GroupMemberEntity> GroupMembers { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{FriendEntity}"/>.
    /// </summary>
    public DbSet<FriendEntity> Friends { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{MessageEntity}"/>.
    /// </summary>
    public DbSet<MessageEntity> Messages { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TenantEntity}"/>.
    /// </summary>
    public DbSet<TenantEntity> Tenants { get; set; }
}
