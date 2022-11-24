namespace HSS.UserApi.Users.Contracts
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public UserModel Profile { get; set; }
    }
}