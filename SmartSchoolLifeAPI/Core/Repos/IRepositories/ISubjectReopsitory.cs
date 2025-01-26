using SmartSchoolLifeAPI.Core.Models.Settings.Subject;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface ISubjectReopsitory : IRepository<SubjectModel>
    {
        dynamic GetTeacherSubjects(string teacherId, int schoolClassId);
    }
}
