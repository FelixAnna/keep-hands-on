using EStore.Common;
using EStore.DataAccess.MemCache.Products;
using EStore.DataAccess.Wrapper.Products;
using EStore.SharedServices.Products.Repositories;
using EStore.SharedServices.Products.Services;
using EStore.SharedServices.SqlServer.Products;

namespace EStore.ProductAPI.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>()!;

            var connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
            services.AddSingleton(settings);
            services.AddMemoryCache();
            services.AddScoped(op => new SqlProductRepository(connectionString));
            services.AddScoped<CachedProductRepository>();
            services.AddScoped<IProductRepository, WrapperProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
