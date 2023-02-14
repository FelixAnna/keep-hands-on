using HSS.IdentityServer.Data;
using HSS.IdentityServer.Models;
using HSS.SharedModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSS.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FakeAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<FakeAuthController> _logger;
        private readonly DbContext _context;

        public FakeAuthController(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<FakeAuthController> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _context = context;
        }

        [HttpPost("signup")]
        public async Task<dynamic> SignUp(FakeAuthInputModel input)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.TenantId = input.TenantId;
                user.NickName = input.NickName;
                user.AvatarUrl = input.AvatarUrl;
                user.IsAdHoc = true;

                await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    return new { UserId = user.Id };
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return new { UserId = "" };
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
