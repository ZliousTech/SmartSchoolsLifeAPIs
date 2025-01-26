using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartSchoolLifeAPI.Models.Settings.Semester;

namespace SmartSchoolLifeAPI.Models.Settings.Exam
{
    public class ExamModel : SemesterModel
    {
        public int SubjectID { get; set; }
        public string SubjectArabicName { get; set; }
        public string SubjectEnglishName { get; set; }
        public int ExamTypeID { get; set; }
        public string ExamTypeArabicName { get; set; }
        public string ExamTypeEnglishName { get; set; }
        public int ExamTitleID { get; set; }
        public string ExamTitleArabicName { get; set; }
        public string ExamTitleEnglishName { get; set; }
        public decimal TotalGrades { get; set; }
        public string TeacherID { get; set; }
        public string TeacherArabicName { get; set; }
        public string TeacherEnglishName { get; set; }
        public bool IsCounted { get; set; }
        public DateTime? ExamDate { get; set; }
    }
}