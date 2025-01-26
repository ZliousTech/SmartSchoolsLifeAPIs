using SmartSchoolLifeAPI.Core.Models.Activity;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface ITeacherActivityRepository : IRepository<TeacherActivity>
    {
        dynamic PrepareTeacherActivity(string teacherId, string schoolClassId, string sectionId, int pageNumber, int pageSize);
    }
}