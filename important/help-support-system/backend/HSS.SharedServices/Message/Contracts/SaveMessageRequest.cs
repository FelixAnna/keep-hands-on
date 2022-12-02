namespace HSS.SharedServices.Message.Contracts
{
    public class SaveMessageRequest
    {
        public string Sender { get; set; } = null!;

        public string Receiver { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
