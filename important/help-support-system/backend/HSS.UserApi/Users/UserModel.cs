namespace HSS.UserApi.Users
{
    public class UserModel
    {
        public string UserId { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
