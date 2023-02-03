using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace EStore.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var consulCondig = configuration.GetRequiredSection("ConsulConfig").Get<ConsulConfig>()!;

            services.AddSingleton(consulCondig);

            services.AddSingleton<IConsulClient>(_ => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(consulCondig.Address);
            }));

            return services;
        }

        public static IServiceCollection AddEStoreAuthentication(this IServiceCollection services)
        {
            // Add services to the container.
            var settings = services.BuildServiceProvider().GetService<IdentityPlatformSettings>()!;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Configure the Authority to the expected value for
                    // the authentication provider. This ensures the token
                    // is appropriately validated.
                    // automic validate
                    options.Audience = settings.Audience;
                    options.Authority = settings.Authority;
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

                    options.TokenValidationParameters.ValidAudiences = new[] { "user", "product", "order", "cart" };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx =>
                        {
                            Console.WriteLine("Exception:" + ctx.Exception);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = ctx =>
                        {
                            Console.WriteLine("Token Validated");
                            return Task.CompletedTask;
                        },
                        OnForbidden = ctx =>
                        {
                            Console.WriteLine("Forbbide:" + ctx.Options.Audience);
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
        public static IServiceCollection AddEStoreAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
                {
                    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                    {
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                        policy.RequireClaim(ClaimTypes.NameIdentifier);
                    });

                    options.AddPolicy("ProductAdmin", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", "product.write");
                    });

                    options.AddPolicy("OrderAdmin", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", "order.admin");
                    });

                    options.AddPolicy("CartAdmin", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", "cart.admin");
                    });

                    options.AddPolicy("Customer", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", "cart.readwrite", "order.readwrite", "product.read");
                    });

                    options.AddPolicy("Anonymous", policy =>
                    {
                        policy.RequireClaim("scope", "product.read");
                    });
                });

            return services;
        }

        public static IServiceCollection AddOpenApiSupport(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer {YourToken}\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                 });
            });
            return services;
        }
    }
}
