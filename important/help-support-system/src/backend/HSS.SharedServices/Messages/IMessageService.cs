using HSS.SharedServices.Messages.Contracts;

namespace HSS.SharedServices.Messages
{
    public interface IMessageService
    {
        List<MessageModel> GetMessages(string from, string to);

        /// <summary>
        /// Get who had talked to user with Id: fromTo
        /// </summary>
        /// <param name="fromTo"></param>
        /// <returns></returns>
        List<MessageModel> GetMessengers(string fromTo);

        List<MessageModel> GetGroupMessages(string groupId);

        Task SaveMessageAsync(SaveMessageRequest request);
    }
}
