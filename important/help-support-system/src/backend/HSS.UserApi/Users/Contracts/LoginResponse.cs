using HSS.SharedModels.Models;

namespace HSS.UserApi.Users.Contracts
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public UserModel Profile { get; set; } = null!;
    }
}