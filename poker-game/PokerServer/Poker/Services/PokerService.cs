using Microsoft.Extensions.Logging;
using PokerServer.Poker.Entity;
using PokerServer.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerServer.Poker
{
    public class PokerService : IPokerService
    {
        private readonly ILogger<PokerService> _logger;

        private List<Command> cmdHistory;
        private readonly PokerStockEntity pokerStock;

        public PokerService(PokerStockEntity pokerStock, ILogger<PokerService> logger)
        {
            _logger = logger;
            this.pokerStock = pokerStock;
            cmdHistory = new List<Command>();
            //Initial();
        }

        public void Initial()
        {
            cmdHistory.Clear();
            pokerStock.Initial(3, 5, 7);
        }

        public bool Pick(string userName, int row, int count)
        {
            var lastCommand = cmdHistory.LastOrDefault(x => x.Succeed);
            if (lastCommand?.UserName == userName)
            {
                _logger.LogWarning($"{userName} already actioned this turn");
                return false;
            }

            var result = pokerStock.Pick(row, count);

            cmdHistory.Add(new Command()
            {
                UserName = userName,
                Count = count,
                Row = row,
                ExecutionTime = DateTime.Now,
                Succeed = result
            });
            return result;
        }

        public bool IsFinished()
        {
            return pokerStock.IsEmpty();
        }

        public string GetWinner()
        {
            if (IsFinished())
            {
                var winnerCommand = cmdHistory.Where(x => x.Succeed).SkipLast(1).LastOrDefault();
                return winnerCommand?.UserName;
            }

            return string.Empty;
        }

        public string Print()
        {
            return pokerStock.Print();
        }
    }
}
