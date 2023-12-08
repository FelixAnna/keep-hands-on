using MovieStudio.Staff.Team;
using MovieStudio.Thirdparty;
using MovieStudio.Thirdparty.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieStudio.Staff.Services
{
    public class StaffingService : IStaffingService
    {
        private readonly List<StudioEmployee> staff;

        public StaffingService()
        {
            staff = new List<StudioEmployee>();
        }

        public void HireNewStaff(params StudioEmployee[] persons)
        {
            staff.AddRange(persons);
        }

        public void HireNewStaff(StudioStaff staff)
        {
            this.staff.AddRange(staff.GetActors());
            this.staff.AddRange(staff.GetCameramen());
        }

        public long GetStaffEstimatedSalary(int daysInProduction, int potentialRisk)
        {
            var ratioOfSalary = (long)Math.Round(100.0 + potentialRisk / 100.0);
            return staff.Sum(x => x.Salary * daysInProduction * ratioOfSalary);
        }

        public override string ToString()
        {
            var staffs = staff.Select(person => person.ToString()).ToArray();

            return string.Join(Environment.NewLine, staffs);
        }

        public IEnumerable<T> GetStaffByProfession<T>() where T : StudioEmployee
        {
            return staff.Where(staff => staff is T).Select(x => (T)x).ToArray();
        }

        public long PayAllStaffs()
        {
            if (!HasAccountant())
            {
                throw new NoSuchProfessionException(nameof(Accountant));
            }

            var totalPaidAmount = 0L;
            var accountant = GetFirstAccountant();
            foreach (var person in staff)
            {
                totalPaidAmount += person.Salary;
                accountant.Pay(person);
            }

            return totalPaidAmount;
        }
        private Accountant GetFirstAccountant()
        {
            return (Accountant)staff.FirstOrDefault(person => person is Accountant);
        }

        private bool HasAccountant()
        {
            return staff.Any(person => person is Accountant);
        }
    }
}
