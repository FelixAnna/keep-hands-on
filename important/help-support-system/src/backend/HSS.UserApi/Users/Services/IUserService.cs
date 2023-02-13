using HSS.UserApi.Users.Contracts;

namespace HSS.UserApi.Users.Services
{
    public interface IUserService
    {
        Task<LoginResponse> PasswordSignInAsync(LoginRequest request);
        Task<FakeLoginResponse> FakeRegisterAsync(int tenantId);
    }
}