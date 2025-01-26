using SmartSchoolLifeAPI.Core.Models;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface ISchoolClassesRepository : IRepository<SchoolClasses>
    {
        dynamic GetSchoolClasses(int schoolId);
        dynamic GetTeacherSchoolClasses(string teacherId, string schoolYear);
    }
}
