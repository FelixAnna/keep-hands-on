using EStore.Common;
using EStore.DataAccess.SqlServer.Carts;
using EStore.DataAccess.SqlServer.Orders;
using EStore.EventServices.Azure;
using EStore.OrderAPI.ApplicationServices;
using EStore.SharedServices.Carts.Repositories;
using EStore.SharedServices.Carts.Services;
using EStore.SharedServices.Orders.Repositories;
using EStore.SharedServices.Orders.Services;
using EStore.SharedServices.Packages;
using EStore.SharedServices.Products.Repositories;
using EStore.SharedServices.SqlServer.Products;

namespace EStore.OrderAPI.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>()!;

            var connectionString = configuration["kse:sqlconn"]!;
            services.AddSingleton(settings);
            services.AddScoped<ICartsRepository, SqlCartsRepository>(op => new SqlCartsRepository(connectionString));
            services.AddScoped<IProductRepository, SqlProductRepository>(op => new SqlProductRepository(connectionString));
            services.AddScoped<IOrdersRepository, SqlOrdersRepository>(op => new SqlOrdersRepository(connectionString));
            services.AddScoped<ICartsService, CartsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<EventGridService>();
            services.AddScoped<OrdersApplicationService>(); 
            services.AddScoped<IPackService, PackageService>();


            return services;
        }
    }
}
