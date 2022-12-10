using HSS.Common;
using HSS.SharedServices.Contacts.Services;
using HSS.SharedServices.Messages;
using HSS.SharedServices.Sql.Contact;
using HSS.SharedServices.Sql.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HSS.HubServer.Extensions
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get values from the config given their key and their target type.
            IdentityPlatformSettings settings = configuration.GetRequiredSection("IdentityPlatformSettings").Get<IdentityPlatformSettings>();

            var connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
            services.AddSingleton(settings);
            services.AddScoped<IContactService, ContactService>(provider => new ContactService(connectionString));
            services.AddScoped<IMessageService, MessageService>(provider => new MessageService(connectionString));
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services)
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
            return services;
        }
    }
}
