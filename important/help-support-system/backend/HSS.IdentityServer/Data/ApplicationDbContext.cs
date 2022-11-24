using HSS.Common.Entities;
using HSS.IdentityServer.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HSS.IdentityServer.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {

    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{GroupContactModel}"/>.
    /// </summary>
    public DbSet<GroupEntity> Groups { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{GroupMembersModel}"/>.
    /// </summary>
    public DbSet<GroupMemberEntity> GroupMembers { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{FriendContactModel}"/>.
    /// </summary>
    public DbSet<FriendEntity> Friends { get; set; }
}
