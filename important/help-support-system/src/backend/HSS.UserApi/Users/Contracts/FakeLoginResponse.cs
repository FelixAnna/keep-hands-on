using HSS.SharedModels.Models;

namespace HSS.UserApi.Users.Contracts
{
    public class FakeLoginResponse : LoginResponse
    {
        public UserModel SupportProfile { get; set; } = null!;
    }
}
