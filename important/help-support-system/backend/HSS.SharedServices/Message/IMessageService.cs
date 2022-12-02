using HSS.SharedServices.Message.Contracts;

namespace HSS.SharedServices.Message
{
    public interface IMessageService
    {
        List<MessageModel> GetMessages(string from, string to);

        List<MessageModel> GetGroupMessages(string groupId);

        Task SaveMessageAsync(SaveMessageRequest request);
    }
}
