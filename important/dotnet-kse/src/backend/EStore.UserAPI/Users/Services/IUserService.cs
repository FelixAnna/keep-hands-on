using EStore.UserAPI.Users.Contracts;

namespace EStore.UserAPI.Users.Services
{
    public interface IUserService
    {
        Task<LoginResponse> PasswordSignInAsync(LoginRequest request);
    }
}