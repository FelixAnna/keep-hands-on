
using HSS.Admin.Data;
using HSS.Admin.Extensions;
using HSS.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

//connect to azure app configurations
builder.Configuration.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("ConnectionString"));
builder.Services.AddDbContext<HSSAdminContext>(options =>options.UseSqlServer(builder.Configuration.GetValue<string>("hss:sqlconn")));

builder.Services.AddServices(builder.Configuration);
builder.Services.AddOpenIdConnect();

// Add services to the container.
builder.Services.AddControllersWithViews();

var retryPolicy = HttpPolicyExtensions
  .HandleTransientHttpError()
  .Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the inner execution times out
  .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);

builder.Services.AddHttpClient("userClient", c => c.BaseAddress = new Uri(uriString: builder.Configuration["UserApiBaseAddress"]!))
  .AddPolicyHandler(retryPolicy)
  .AddPolicyHandler(timeoutPolicy);

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

app.Run();