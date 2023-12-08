using MovieStudio.Interfaces;
using MovieStudio.Thirdparty;
using System;

namespace MovieStudio.Staff
{
    public class Actor : StudioEmployee, IActor
    {
        public Actor(string name) : base(name, JobSalary.ACTOR)
        {
        }

        public Actor(string name, long salary) : base(name, salary)
        {

        }

        public virtual bool Act()
        {
            return new Random().NextDouble() > 0.04;
        }

        public override string ToString()
        {
            return $"{nameof(Actor)}: {Name}, earned money:{EarnedMoney}, salary: {Salary}\n";
        }
    }
}
