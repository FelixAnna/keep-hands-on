using EStore.CMS.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);
builder.Services.AddOpenIdConnect();

// Add services to the container.
builder.Services.AddControllersWithViews();
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
