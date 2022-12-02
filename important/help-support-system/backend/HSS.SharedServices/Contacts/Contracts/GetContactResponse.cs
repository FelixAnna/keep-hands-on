namespace HSS.SharedServices.Contacts.Contracts
{
    public class GetContactResponse
    {
        public string UserId { get; set; } = null!;

        public UserContactModel Contact { get; set; } = null!;
    }
}
