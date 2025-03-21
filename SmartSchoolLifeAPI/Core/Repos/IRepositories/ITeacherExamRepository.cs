using SmartSchool.FireBase.Service;
using SmartSchoolLifeAPI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface ITeacherExamRepository : IRepository<Exam>
    {
        dynamic GetExamsForTeacher(string teacherId, int schoolID, string semesterID,
            string schoolClassId, string sectionId, string subjectId, string examTypeId, string schoolYear, int pageNumber, int pageSize);
        void AddOrUpdateExamsDates(Dictionary<int, string> examsDates);

        Task<Exam> AddAsync(Exam model, DeviceType deviceType);
    }
}