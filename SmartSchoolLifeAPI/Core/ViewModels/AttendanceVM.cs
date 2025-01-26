namespace SmartSchoolLifeAPI.ViewModels
{
    public class AttendanceVM
    {
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public string StudentID { get; set; }
        public string AttendanceDate { get; set; }
        public int AttendanceType { get; set; }
        public int AbsenseReason { get; set; }
        public string Description { get; set; }
        public int ClassOrder { get; set; }
        public string ClassOrderTime { get; set; }
    }
}