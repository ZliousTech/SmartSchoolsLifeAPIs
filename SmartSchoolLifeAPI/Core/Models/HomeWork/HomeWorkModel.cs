namespace SmartSchoolLifeAPI.Core.Models.HomeWork
{
    public class HomeWorkModel
    {
        public int HomeWorkID { set; get; }
        public int SchoolClassID { set; get; }
        public int SectionID { set; get; }
        public int SubjectID { set; get; }
        public string HomeWorkTitle { set; get; }
        public string HomeWorkCreationDate { set; get; }
        public string HomeWorkDeadLine { set; get; }

        public string HomeWorkAttachment { set; get; }
        public string Ext { set; get; }
        public string HomeWorkNote { set; get; }
        public string TeacherID { set; get; }
    }
}
