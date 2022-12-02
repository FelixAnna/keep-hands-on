using System.Text.Json.Serialization;

namespace HSS.SharedServices.Messages
{
    public class MessageModel
    {
        public int Id { get; set; }

        public string From { get; set; } = null!;

        public string To { get; set; } = null!;

        public string Content { get; set; } = null!;

        [JsonPropertyName("msg_time")]
        public DateTime? MsgTime { get; set; }
    }
}
