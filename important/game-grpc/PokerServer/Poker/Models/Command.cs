using System;

namespace PokerServer.Poker.Models
{
    public class Command
    {
        public string UserName;

        public int Row;
        public int Count;

        public DateTime ExecutionTime;

        public bool Succeed;
    }
}
