using MovieStudio.Thirdparty.Exceptions;

namespace MovieStudio.Finance
{
    public class MovieBudget
    {
        private readonly long initialBudgeMoney;

        private long budgetMoney;

        public MovieBudget(long budgetMoney)
        {
            this.initialBudgeMoney = budgetMoney;
            this.budgetMoney = budgetMoney;
        }

        public void Decrease(long paidSum)
        {
            if (budgetMoney < paidSum)
            {
                throw new BudgetIsOverException();
            }

            budgetMoney -= paidSum;
        }

        public long GetRemainingBudget()
        {
            return budgetMoney;
        }

        public long GetBudgetSpent()
        {
            return initialBudgeMoney - budgetMoney;
        }

        public override string ToString()
        {
            return $"Budget: {initialBudgeMoney} initial, {GetBudgetSpent()} spent, {budgetMoney} economy\n";
        }
    }
}
