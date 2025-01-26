using SmartSchoolLifeAPI.Core.Models;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface IAcademicCalendarRepository : IRepository<AcademicCalendar>
    {
        dynamic GetStudentCalendar(string studentId, int schoolId);
    }
}