using SmartSchoolLifeAPI.Core.Models.Grades;
using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface IGradesRepository : IRepository<GradesModel>
    {
        IEnumerable<GradesViewModel> GetStudentGrades(string studentId);
        IEnumerable<dynamic> GetTeacherGrades(int semesterID, int sectionID, int subjectID,
            int examTypeID, int examTitleID);
        void AddStudentsGrades(Dictionary<int, double> studentsGrades);
    }
}
