using System;

namespace SmartSchoolLifeAPI.ViewModels
{
    public class TeacherClasses
    {
        public int SectionID { get; set; }
        public int SchoolClassID { get; set; }
        public string TeacherID { get; set; }
        public string SubjectEnglishName { get; set; }
        public string SectionEnglishName { get; set; }
        public string SchoolClassEnglishName { get; set; }
        public string StaffEnglishName { get; set; }
        public int SessionDayOrder { get; set; }
        public DateTime SessionTime { get; set; }
    }
}