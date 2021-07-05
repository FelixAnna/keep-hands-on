using Microsoft.Extensions.Logging;
using PokerServer.Poker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerServer.Poker.Entity
{
    public class PokerStockEntity
    {
        private readonly ILogger<PokerStockEntity> _logger;
        private Dictionary<int, List<PokerModel>> _pokers;
        public PokerStockEntity(ILogger<PokerStockEntity> logger)
        {
            _logger = logger;
        }

        public void Initial(params int[] capacities)
        {
            _pokers = new Dictionary<int, List<PokerModel>>();
            int index = 1;
            foreach(var ca in capacities)
            {
                if (ca <= 0) continue;

                var values = new List<PokerModel>();
                for (var i=0; i<ca; i++)
                {
                    values.Add(new PokerModel());
                }

                _pokers.Add(index++, values);
            }
        }

        public bool Pick(int row, int count)
        {
            if (!_pokers.ContainsKey(row))
            {
                _logger.LogWarning("Selected row does not exists");
                return false;
            }
            if (_pokers[row].Count()<count)
            {
                _logger.LogWarning("Selected row does not have enough pokers");
                return false;
            }

            _pokers[row].RemoveRange(0, count);
            return true;
        }

        public bool IsEmpty()
        {
            return !_pokers.SelectMany(x => x.Value).Any();
        }

        public string Print()
        {
            var strBuilder = new StringBuilder();
            foreach (var kv in _pokers)
            {
                strBuilder.Append($"line {kv.Key}:"); 
                strBuilder.Append(' ');

                foreach (var p in kv.Value)
                {
                    strBuilder.Append(p.ToString());
                    strBuilder.Append(' ');
                }

                strBuilder.Append($"    ({kv.Value.Count})");
                strBuilder.AppendLine();
            }

            return strBuilder.ToString();
        }
    }
}
