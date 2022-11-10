using HSS.SignalRDemo.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace HSS.SignalRDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            var httpClient = new HttpClient();
            var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = "https://localhost:8443/connect/token",
                GrantType = "password",

                ClientId = "webclient2",
                ClientSecret = "SomethingUnknown",
                Scope = "b",
                
                UserName = user.UserName,
                Password = user.Password
            });

            if (!identityServerResponse.IsError)
            {
                Console.WriteLine("token: {0}", identityServerResponse.AccessToken);
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
        public IActionResult Profile()
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result ?? "";
            //string idToken = HttpContext.GetTokenAsync("id_token").Result ?? "";

            ViewData["userName"] = (User.FindFirst(ClaimTypes.Name)?.Value);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}