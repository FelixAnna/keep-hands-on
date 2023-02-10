using HSS.SharedServices.Messages.Contracts;

namespace HSS.SharedServices.Messages
{
    public interface IMessageService
    {
        List<MessageModel> GetMessages(string from, string to);

        List<MessageModel> GetGroupMessages(string groupId);

        Task SaveMessageAsync(SaveMessageRequest request);
    }
}
