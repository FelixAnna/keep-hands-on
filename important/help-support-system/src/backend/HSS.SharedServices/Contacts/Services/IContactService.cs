using HSS.SharedServices.Contacts.Contracts;

namespace HSS.SharedServices.Contacts.Services
{
    public interface IContactService
    {
        GetContactResponse GetUserContact(string userId);

        GetColleagueResponse GetColleagues(string userId, string keywords);
    }
}
