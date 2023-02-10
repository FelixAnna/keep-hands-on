namespace HSS.SharedServices.Groups.Contracts
{
    public class GetGroupMemebersResponse
    {
        public Group Group { get; set; } = null!;

        public List<GroupMember>? Memebers { get; set; }
    }
    public class GroupMember
    {
        public string UserId { get; set; } = null!;

        public string NickName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int TenantId { get; set; }
    }
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = null!;
    }
}
