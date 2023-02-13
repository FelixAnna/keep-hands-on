using HSS.Common;
using HSS.SignalRDemo.Models;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HSS.SignalRDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IdentityPlatformSettings settings;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;

        public HomeController(IdentityPlatformSettings settings,
            ILogger<HomeController> logger,
            IConfiguration configuration)
        {
            this.settings = settings;
            _logger = logger;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserModel user)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(settings.Authority);
            /*
             * var client = new HttpClient();

             * 
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");
            */
            var httpClient = new HttpClient();
            var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = "password",

                ClientId = "webclient2",
                ClientSecret = "SomethingUnknown@1116",
                //Scope = "read",

                UserName = user.UserName,
                Password = user.Password
            });

            if (!identityServerResponse.IsError)
            {
                var userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = disco.UserInfoEndpoint,

                    Token = identityServerResponse.AccessToken,
                });
                Console.WriteLine("User: {0}", userInfoResponse.Json);
                ViewData["user"] = userInfoResponse.Json;
                ViewData["token"] = identityServerResponse.AccessToken;
                return View();
            }

            return RedirectToAction("Error");
        }

        [Authorize]
        public IActionResult Chat()
        {
            return View();
        }

        [Authorize]
        public IActionResult GroupChat()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result ?? "";

            ViewData["id"] = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ViewData["userName"] = (User.FindFirst(JwtClaimTypes.NickName)?.Value);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("hub")]
        public IActionResult GetHub()
        {
            return Ok(configuration["SignalRHub"]);
        }
    }
}