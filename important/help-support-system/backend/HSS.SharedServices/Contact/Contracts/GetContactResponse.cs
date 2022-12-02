namespace HSS.SharedServices.Contact.Contracts
{
    public class GetContactResponse
    {
        public string UserId { get; set; }

        public UserContactModel Contact { get; set; }
    }
}
