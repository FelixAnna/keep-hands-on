
## startup project
dotnet new react -au Individual -n EStore.IdentityServer

refer: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-6.0


## change email verifaction to false

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

## allow auth server itself & change port to fixed(like: 8443)
    builder.Services.Configure<JwtBearerOptions>(
    IdentityServerJwtConstants.IdentityServerJwtBearerScheme,
    options =>
    {
        options.Authority = "https://localhost:8443";
    });

    change: ClientApp/.env.deployment port to: PORT=8443

    <SpaProxyServerUrl>https://localhost:8443</SpaProxyServerUrl>

## config idp with skip pkce for client or by using appsettings.json
```C#
    var webClient = new Client()
    {
        ClientId = "webclient",
        ClientName = "Example client application using client credentials or code",
        AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
        ClientSecrets = new List<Secret> { new Secret("SomethingUnknown".Sha256()) }, // change me!
        AllowedScopes = new List<string> { "a" },
        RequirePkce = false,
        AlwaysIncludeUserClaimsInIdToken = true,
        RedirectUris = new[] { "http://localhost:3000/login/test" },
        PostLogoutRedirectUris = { "http://localhost:3000/logout/test" }
    };

    var webClient2 = new Client()
    {
        ClientId = "webclient2",
        ClientName = "Example client application using passowrd",
        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
        ClientSecrets = new List<Secret> { new Secret("SomethingUnknown".Sha256()) }, // change me!
        AllowedScopes = new List<string> { "b" },
        RequirePkce = false,
        RedirectUris = new[] { "http://localhost:3000/login/test" },
        PostLogoutRedirectUris = { "http://localhost:3000/logout/test" },
    };

    builder.Services.AddIdentityServer()
        .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
        {
            options.Clients.Add(webClient);
            options.Clients.Add(webClient2);

            options.ApiResources.AddApiResource("extenalAPi", resource =>
                resource.WithScopes("a", "b", "c"));
        });
```
## switch sqlite to sql server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

## fix dependencies issues
fix using/other issues, make sure build success

## migration
Update migrations:

    Remove-Migration
    Add-Migration InitialCreate
    Update-Database

    dotnet ef migrations add migrationName
    dotnet ef database update
    dotnet ef database update migrationName  -- rollback to special migration

    dotnet ef database remove

