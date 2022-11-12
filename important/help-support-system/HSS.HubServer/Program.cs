using HSS.HubServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    // Configure the Authority to the expected value for
    // the authentication provider. This ensures the token
    // is appropriately validated.
    // automic validate
    options.Audience = "HSS.IdentityServerAPI"; 
    options.Authority = "https://localhost:8443"; // TODO: Update URL

    /* manually validation
    var client = new HttpClient();
    var disco = client.GetDiscoveryDocumentAsync("https://localhost:8443").Result;

    var keys = new List<SecurityKey>();
    foreach (var webKey in disco.KeySet.Keys)
    {
        var e = Base64Url.Decode(webKey.E);
        var n = Base64Url.Decode(webKey.N);

        var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
        {
            KeyId = webKey.Kid
        };

        keys.Add(key);
    }

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // The signing key must match!
        ValidateIssuerSigningKey = true,
        IssuerSigningKeys = keys,

        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,

        RequireSignedTokens = true,

        // Validate the JWT Issuer (iss) claim
        ValidateIssuer = true,
        ValidIssuer = "https://localhost:8443",

        // Validate the JWT Audience (aud) claim
        ValidateAudience = true,
        ValidAudience = "HSS.IdentityServerAPI",

        // Validate the token expiry
        RequireExpirationTime = true,
        ValidateLifetime = true,

        // If you want to allow a certain amount of clock drift, set that here:
        ClockSkew = TimeSpan.FromMinutes(5)
    };
    */


    // We have to hook the OnMessageReceived event in order to
    // allow the JWT authentication handler to read the access
    // token from the query string when a WebSocket or 
    // Server-Sent Events request comes in.

    // Sending the access token in the query string is required due to
    // a limitation in Browser APIs. We restrict it to only calls to the
    // SignalR hub in this code.
    // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
    // for more information about security considerations when using
    // the query string to transmit the access token.
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
    builder.WithOrigins("https://localhost:7076")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapHub<Chat>("/chat");

app.Run();
