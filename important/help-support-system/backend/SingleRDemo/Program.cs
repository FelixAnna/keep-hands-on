using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie(JwtBearerDefaults.AuthenticationScheme)
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:8443";
        options.ClientId = "webclient3";
        options.ClientSecret = "SomethingUnknown";
        options.ResponseType = "code";
        options.UsePkce = false;
        options.ResponseMode = "query";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("HSS.IdentityServerAPI");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.NonceCookie.SameSite = SameSiteMode.Unspecified;
        options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.UseCors(builder =>
{
    builder.WithOrigins("https://localhost:7076")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
