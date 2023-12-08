using MovieStudio.Thirdparty;
using System;

namespace MovieStudio.Staff
{
    public class SuperActor : Actor
    {
        private const double ActionFailedRate = 0.01;

        public SuperActor(string name) : base(name, JobSalary.SUPERSTAR)
        {
        }

        public override bool Act()
        {
            return new Random().NextDouble() > ActionFailedRate;
        }

        public override string ToString()
        {
            return $"{nameof(SuperActor)}: {Name}, earned money:{EarnedMoney}, salary: {Salary}\n";
        }
    }
}
