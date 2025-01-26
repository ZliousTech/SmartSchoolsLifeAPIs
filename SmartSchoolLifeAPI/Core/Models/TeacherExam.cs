namespace SmartSchoolLifeAPI.Core.Models
{
    public class Exam
    {
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SchoolClassID { get; set; }
        public int SectionID { get; set; }
        public int SemesterID { get; set; }
        public int SubjectID { get; set; }
        public int ExamTypeID { get; set; }
        public int ExamTitleID { get; set; }
        public decimal TotalGrades { get; set; }
        public string TeacherID { get; set; }
        public bool IsCounted { get; set; }
        public string ExamDate { get; set; }
    }
}