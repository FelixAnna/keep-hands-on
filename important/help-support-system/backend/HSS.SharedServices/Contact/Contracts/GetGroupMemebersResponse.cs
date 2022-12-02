namespace HSS.SharedServices.Contact.Contracts
{
    public class GetGroupMemebersResponse
    {
        public GroupModel Group { get; set; }

        public List<GroupMember> Memebers { get; set;}
    }
    public class GroupMember
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get;set; }
    }
}
