using EStore.Common.Extensions;
using EStore.Common.Middlewares;
using EStore.OrderAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration);
builder.Services.AddConsulConfig(builder.Configuration);
builder.Services.AddEStoreAuthentication();
builder.Services.AddEStoreAuthorization();

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
app.Lifetime.ApplicationStarted.Register(() => app.RegisterWithConsul(app.Lifetime));
app.Run();

