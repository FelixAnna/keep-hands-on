using MovieStudio.Interfaces;
using MovieStudio.Thirdparty;
using System;

namespace MovieStudio.Staff
{
    public class Recruiter : StudioEmployee, IRecruiter
    {
        public Recruiter(string name) : base(name, JobSalary.RECRUITER)
        {
        }

        public StudioEmployee Hire<T>(string name, Func<string, T> createInstance) where T : StudioEmployee
        {
            return createInstance(name);
        }
        public override string ToString()
        {
            return $"{nameof(Recruiter)}: {Name}, earned money:{EarnedMoney}, salary: {Salary}\n";
        }
    }
}
