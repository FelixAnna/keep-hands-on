using HSS.Common;
using HSS.HubServer;
using HSS.HubServer.EFCoreGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IdentityPlatformSettings settings = builder.Configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>();

builder.Services.AddDbContext<SshDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    // Configure the Authority to the expected value for
    // the authentication provider. This ensures the token
    // is appropriately validated.
    // automic validate
    options.Audience = settings.Audience; 
    options.Authority = settings.Authority;
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidTypes = new[] { "at+jwt" }
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
                
            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            Console.WriteLine("Hub get token: " + accessToken.ToString() + " , path is start with /chat:" + path.StartsWithSegments("/chat"));
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
            {
                Console.WriteLine("token set");
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine("EEEEEEEEEEEE" + ctx.Exception);
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            Console.WriteLine("tokenvalidated" + ctx.Result);
            return Task.CompletedTask;
        },
        OnForbidden = ctx =>
        {
            Console.WriteLine("Forbbiden my block reason " + ctx.Options.Audience);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors();

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
});
//builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapHub<Chat>("/chat");

app.Map("/status", () => "hello");

app.Run();
