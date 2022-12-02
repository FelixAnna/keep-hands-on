namespace HSS.SharedServices.Contact
{
    public class UserContactModel
    {
        public IEnumerable<GroupModel> Groups { get; set; }

        public IEnumerable<FriendModel> Friends { get; set; }
    }

    public class GroupModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class FriendModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}