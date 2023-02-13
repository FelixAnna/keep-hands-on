namespace HSS.SharedModels.Models
{
    public class FakeAuthInputModel
    {
        public int? TenantId { get; set; }
        public string? NickName { get; set; }
        public string? AvatarUrl { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
