using HSS.HubServer;
using HSS.HubServer.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuth();

// Add services to the container.
builder.Services.AddRazorPages();
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