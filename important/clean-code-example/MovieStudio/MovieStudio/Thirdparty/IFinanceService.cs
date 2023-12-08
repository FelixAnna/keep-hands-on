using MovieStudio.Staff;
using MovieStudio.Staff.Services;

namespace MovieStudio.Thirdparty
{
    public interface IFinanceService
    {
        void InitBudget(long initialSum);

        void DecreaseBudget(long paidSum);

        long GetBudget();

        void PaySalary(StaffingService staffingService);

    }
}
