namespace SmartSchoolLifeAPI.Core.Models.Grades
{
    public class GradesViewModel
    {
        public string SemesterArabicName { set; get; }
        public string SemesterEnglishName { set; get; }
        public string SubjectArabicName { set; get; }
        public string SubjectEnglishName { set; get; }

        ///* Shared */
        //public string TypeArabicName { set; get; }
        //public string TypeEnglishName { set; get; }
        //public string ExamTitleArabicName { set; get; }
        //public string ExamTitleEnglishName { set; get; }
        //public decimal GradeValue { set; get; }
        //public decimal TotalGrades { set; get; }
        //public string ExamDate { set; get; }
        //public string StaffArabicName { set; get; }
        //public string StaffEnglishName { set; get; }
        //public string ExamStatus { set; get; }

        /* First */
        public string TypeArabicNameFirst { set; get; }
        public string TypeEnglishNameFirst { set; get; }
        public string ExamTitleArabicNameFirst { set; get; }
        public string ExamTitleEnglishNameFirst { set; get; }
        public decimal GradeValueFirst { set; get; }
        public decimal TotalGradesFirst { set; get; }
        public string ExamDateFirst { set; get; }
        public string StaffArabicNameFirst { set; get; }
        public string StaffEnglishNameFirst { set; get; }
        public string ExamStatusFirst { set; get; }

        /* Second */
        public string TypeArabicNameSecond { set; get; }
        public string TypeEnglishNameSecond { set; get; }
        public string ExamTitleArabicNameSecond { set; get; }
        public string ExamTitleEnglishNameSecond { set; get; }
        public decimal GradeValueSecond { set; get; }
        public decimal TotalGradesSecond { set; get; }
        public string ExamDateSecond { set; get; }
        public string StaffArabicNameSecond { set; get; }
        public string StaffEnglishNameSecond { set; get; }
        public string ExamStatusSecond { set; get; }

        /* Third */
        public string TypeArabicNameThird { set; get; }
        public string TypeEnglishNameThird { set; get; }
        public string ExamTitleArabicNameThird { set; get; }
        public string ExamTitleEnglishNameThird { set; get; }
        public decimal GradeValueThird { set; get; }
        public decimal TotalGradesThird { set; get; }
        public string ExamDateThird { set; get; }
        public string StaffArabicNameThird { set; get; }
        public string StaffEnglishNameThird { set; get; }
        public string ExamStatusThird { set; get; }

        /* Quiz */
        public string TypeArabicNameQuiz { set; get; }
        public string TypeEnglishNameQuiz { set; get; }
        public string ExamTitleArabicNameQuiz { set; get; }
        public string ExamTitleEnglishNameQuiz { set; get; }
        public decimal GradeValueQuiz { set; get; }
        public decimal TotalGradesQuiz { set; get; }
        public string ExamDateQuiz { set; get; }
        public string StaffArabicNameQuiz { set; get; }
        public string StaffEnglishNameQuiz { set; get; }
        public string ExamStatusQuiz { set; get; }

        /* Final */
        public string TypeArabicNameFinal { set; get; }
        public string TypeEnglishNameFinal { set; get; }
        public string ExamTitleArabicNameFinal { set; get; }
        public string ExamTitleEnglishNameFinal { set; get; }
        public decimal GradeValueFinal { set; get; }
        public decimal TotalGradesFinal { set; get; }
        public string ExamDateFinal { set; get; }
        public string StaffArabicNameFinal { set; get; }
        public string StaffEnglishNameFinal { set; get; }
        public string ExamStatusFinal { set; get; }
    }
}