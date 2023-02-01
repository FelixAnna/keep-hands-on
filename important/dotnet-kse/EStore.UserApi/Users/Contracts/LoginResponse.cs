namespace EStore.UserApi.Users.Contracts
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public UserModel Profile { get; set; } = null!;
    }
}