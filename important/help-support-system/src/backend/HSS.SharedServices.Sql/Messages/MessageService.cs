using Dapper;
using Dapper.Contrib.Extensions;
using HSS.SharedModels.Entities;
using HSS.SharedServices.Messages;
using HSS.SharedServices.Messages.Contracts;
using Microsoft.Data.SqlClient;

namespace HSS.SharedServices.Sql.Messages
{
    public class MessageService : IMessageService
    {
        private readonly string connectionString;

        public MessageService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<MessageModel> GetGroupMessages(string groupId)
        {
            using var connnection = new SqlConnection(connectionString);
            var builder = new SqlBuilder();

            var select = builder.AddTemplate("select id, [from], [to], content, msg_time as MsgTime from hss.messages /**where**/ order by msg_time");
            var parameter = new DynamicParameters();

            parameter.Add("groupId", groupId);
            builder.Where(" [to] = @groupId ");

            var results = connnection.Query<MessageModel>(select.RawSql, parameter);

            return results.ToList();
        }

        public List<MessageModel> GetMessages(string from, string to)
        {
            using var connnection = new SqlConnection(connectionString);
            var builder = new SqlBuilder();

            var select = builder.AddTemplate("select id, [from], [to], content, msg_time as MsgTime from hss.messages /**where**/ order by msg_time");
            var parameter = new DynamicParameters();

            parameter.Add("from", from);
            parameter.Add("to", to);
            builder.Where(" ([from] = @from and [to] = @to) or ([to] = @from and [from] = @to) ");

            var results = connnection.Query<MessageModel>(select.RawSql, parameter);

            return results.ToList();
        }

        public List<MessageModel> GetMessengers(string fromTo)
        {
            using var connnection = new SqlConnection(connectionString);
            var builder = new SqlBuilder();

            var select = builder.AddTemplate("select distinct [from], [to] from hss.messages /**where**/ ");
            var parameter = new DynamicParameters();

            parameter.Add("fromTo", fromTo);
            builder.Where(" [from] = @fromTo or [to] = @fromTo ");

            var results = connnection.Query<MessageModel>(select.RawSql, parameter);

            return results.ToList();
        }

        public async Task SaveMessageAsync(SaveMessageRequest request)
        {
            using var connnection = new SqlConnection(connectionString);
            SqlMapperExtensions.TableNameMapper = (type) =>
            {
                return "hss.Messages";
            };

            //TODO fix the schema issue
            await connnection.InsertAsync(new MessageEntity
            {
                From = request.Sender,
                To = request.Receiver,
                Content = request.Content,
                Msg_Time = DateTime.UtcNow
            });
        }
    }
}
