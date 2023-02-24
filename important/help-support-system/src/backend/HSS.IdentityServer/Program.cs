using HSS.Common.Extensions;
using HSS.IdentityServer.Data;
using HSS.IdentityServer.IdentityConfigurations;
using HSS.IdentityServer.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//connect to azure app configurations
builder.Configuration.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("ConnectionString"));

builder.Services.AddCors();
// Add services to the container.
var connectionString = builder.Configuration.GetValue<string>("hss:sqlconn");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddConsulConfig(builder.Configuration);

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(op =>
    {
        var section = builder.Configuration.GetSection("IdentityServer:OtherClients");
        var clients = new List<Client>();
        section.Bind(clients);

        op.Clients.AddRange(clients.ToArray());

        var apiSection = builder.Configuration.GetSection("IdentityServer:ApiResources");
        var apiResources = new List<ApiResource>();
        apiSection.Bind(apiResources);
        op.ApiResources.AddRange(apiResources.ToArray());


        //op.IdentityResources.Clear();
        //op.IdentityResources.AddRange(IdentityServerConfiguration.GetIdentityResources().ToArray());
    })
    .AddProfileService<ProfileService>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.Configure<JwtBearerOptions>(
    IdentityServerJwtConstants.IdentityServerJwtBearerScheme,
    options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("IdentityServer:Authority");
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // These three subnets encapsulate the applicable Azure subnets. At the moment, it's not possible to narrow it down further.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseForwardedHeaders(); //Detect HTTPS session: https://learn.microsoft.com/en-US/azure/app-service/configure-language-dotnetcore?pivots=platform-linux#detect-https-session
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Lifetime.ApplicationStarted.Register(() => app.RegisterWithConsul(app.Lifetime, app.Environment.IsDevelopment()));

app.Run();
