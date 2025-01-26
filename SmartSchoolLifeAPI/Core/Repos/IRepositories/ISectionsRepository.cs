using SmartSchoolLifeAPI.Core.Models;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface ISectionsRepository : IRepository<Sections>
    {
        dynamic GetSchoolClassSections(int schoolID, int schoolClassID);
        dynamic GetTeacherSections(string teacherId, int schoolClassId);
        dynamic GetSectionStudents(int sectionId);
    }
}
