using HSS.Common.Extensions;
using HSS.Common.Middlewares;
using HSS.MessageApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//connect to azure app configurations
builder.Configuration.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("ConnectionString"));

builder.Services.AddServices(builder.Configuration);
builder.Services.AddConsulConfig(builder.Configuration);
builder.Services.AddHSSAuthentication();
builder.Services.AddHSSAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenApiSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.Map("/status", () => "hello");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Lifetime.ApplicationStarted.Register(() => app.RegisterWithConsul(app.Lifetime, app.Environment.IsDevelopment()));
app.Run();
