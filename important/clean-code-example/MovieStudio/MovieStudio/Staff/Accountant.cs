using MovieStudio.Interfaces;
using MovieStudio.Thirdparty;

namespace MovieStudio.Staff
{
    public class Accountant : StudioEmployee, IAccountant
    {
        public Accountant(string name) : base(name, JobSalary.ACCOUNTANT)
        {
        }

        public void Pay(StudioEmployee person)
        {
            person.PaySalary(person.Salary);
        }

        public override string ToString()
        {
            return $"{nameof(Accountant)}: {Name}, earned money:{EarnedMoney}, salary: {Salary}\n";
        }
    }
}
