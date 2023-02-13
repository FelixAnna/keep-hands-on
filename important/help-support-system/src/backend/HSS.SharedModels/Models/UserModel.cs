namespace HSS.SharedModels.Models
{
    public class UserModel
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public int TenantId { get; set; }
    }
}