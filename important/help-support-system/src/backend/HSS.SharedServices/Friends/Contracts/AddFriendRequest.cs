namespace HSS.SharedServices.Friends.Contracts
{
    public class AddFriendRequest
    {
        public string UserId { get; set; } = null!;
        public string TargetUserId { get; set; } = null!;
    }
}