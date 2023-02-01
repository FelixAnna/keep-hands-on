using EStore.UserApi.Users.Contracts;

namespace EStore.UserApi.Users.Services
{
    public interface IUserService
    {
        Task<LoginResponse> PasswordSignInAsync(LoginRequest request);
    }
}