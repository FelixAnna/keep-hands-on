using HSS.Common;
using HSS.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Get values from the config given their key and their target type.
IdentityPlatformSettings settings = builder.Configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>();
// Add services to the container.
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = settings.Authority;
        options.ClientId = settings.ClientId;
        options.ClientSecret = settings.ClientSecret;
        options.ResponseType = "code";
        options.UsePkce = false;
        options.ResponseMode = "query";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("message.readwrite");
        options.Scope.Add("user.contact");
        options.Scope.Add("user.read");
        options.SaveTokens = true;
    })
    .AddJwtBearer(options =>
    {
        // Configure the Authority to the expected value for
        // the authentication provider. This ensures the token
        // is appropriately validated.
        // automic validate
        options.Audience = settings.Audience;
        options.Authority = settings.Authority;
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

builder.Services.AddSingleton(settings);
builder.Services.AddConsulConfig(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    });
});
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
if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders(); //Detect HTTPS session: https://learn.microsoft.com/en-US/azure/app-service/configure-language-dotnetcore?pivots=platform-linux#detect-https-session
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.Map("/status", () => "hello");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Lifetime.ApplicationStarted.Register(() => app.RegisterWithConsul(app.Lifetime));

app.Run();
