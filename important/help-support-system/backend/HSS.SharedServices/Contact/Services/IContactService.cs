using HSS.SharedServices.Contact.Contracts;

namespace HSS.SharedServices.Contact.Services
{
    public interface IContactService
    {
        GetContactResponse GetUserContact(string userId);

        GetGroupMemebersResponse GetGroupMembers(string userId, string groupId);
    }
}
