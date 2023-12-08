using MovieStudio.Interfaces;
using MovieStudio.Thirdparty;
using System;

namespace MovieStudio.Staff
{
    public class CameraMan : StudioEmployee, ICameraMan
    {
        private const double FailedRate = 0.04;

        public CameraMan(string name) : base(name, JobSalary.CAMERA_MAN)
        {
        }

        public bool Shoot()
        {
            return new Random().NextDouble() > FailedRate;
        }

        public override string ToString()
        {
            return $"{nameof(CameraMan)}: {Name}, earned money:{EarnedMoney}, salary: {Salary}\n";
        }
    }
}
