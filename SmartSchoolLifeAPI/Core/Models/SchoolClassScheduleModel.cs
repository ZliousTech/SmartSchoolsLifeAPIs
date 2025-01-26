namespace SmartSchoolLifeAPI.Core.Models
{
    public class SchoolClassScheduleModel
    {
        public int ID { get; set; }
        public string TeacherID { get; set; }
        public string StaffArabicName { get; set; }
        public string StaffEnglishName { get; set; }
        public string SchoolClassArabicName { get; set; }
        public string SchoolClassEnglishName { get; set; }
        public string SectionCode { get; set; }
        public string SectionArabicName { get; set; }
        public string SectionEnglishName { get; set; }
        public string SubjectArabicName { get; set; }
        public string SubjectEnglishName { get; set; }
        public int WeekDay { get; set; }
        public int SessionDayOrder { get; set; }
        public string ItemRGBColor { get; set; }
    }
}
