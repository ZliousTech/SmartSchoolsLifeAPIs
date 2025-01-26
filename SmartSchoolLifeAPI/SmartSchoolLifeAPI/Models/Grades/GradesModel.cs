using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SmartSchoolLifeAPI.Models.Shared;
using SmartSchoolLifeAPI.Models.Settings.Subject;

namespace SmartSchoolLifeAPI.Models.Grades
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