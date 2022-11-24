using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SingleRDemo.Controllers
{
    [Authorize]
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var t = HttpContext.GetTokenAsync("access_token").Result;
            return View("/Pages/Index.cshtml");
        }
    }
}
