using MovieStudio.Staff.Team;
using MovieStudio.Thirdparty;
using System.Collections.Generic;

namespace MovieStudio.Staff.Services
{
    public interface IStaffingService
    {
        IEnumerable<T> GetStaffByProfession<T>() where T : StudioEmployee;
        long GetStaffEstimatedSalary(int daysInProduction, int potentialRisk);
        void HireNewStaff(params StudioEmployee[] persons);
        void HireNewStaff(StudioStaff staff);
        long PayAllStaffs();
        string ToString();
    }
}