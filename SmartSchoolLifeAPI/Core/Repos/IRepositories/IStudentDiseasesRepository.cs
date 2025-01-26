using SmartSchoolLifeAPI.Core.Models.Student.StudentDiseases;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface IStudentDiseasesRepository : IRepository<StudentDiseasesModel>
    {
        dynamic GetStudentDiseases(string internalStudentID);
    }
}
