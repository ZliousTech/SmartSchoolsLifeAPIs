using SmartSchoolLifeAPI.Core.Models.Settings.Subject;

namespace SmartSchoolLifeAPI.Core.Models.Grades
{
    public class GradesModel : SubjectModel
    {
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public decimal GradeValue { get; set; }
        public decimal ExamMaxGrade { get; set; }
    }
}