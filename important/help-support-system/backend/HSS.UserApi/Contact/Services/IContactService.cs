using HSS.UserApi.Contact.Contracts;

namespace HSS.UserApi.Contact.Services
{
    public interface IContactService
    {
        GetContactResponse GetUserContact(string userId);
    }
}
