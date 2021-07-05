using System;

namespace PokerServer.Poker.Models
{
    public class PokerModel
    {
        private static readonly string[] PokerValues = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        private static readonly Random random = new(DateTime.Now.Millisecond);

        public PokerModel()
        {
            var index = random.Next(1, 13);
            Value = PokerValues[index];
        }

        public string Value { get; set; }
        public override string ToString()
        {
            return Value;
        }
    }
}
