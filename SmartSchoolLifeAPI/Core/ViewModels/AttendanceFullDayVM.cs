namespace SmartSchoolLifeAPI.ViewModels
{
    public class AttendanceFullDayVM
    {
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public string StudentID { get; set; }
        public string AttendanceDate { get; set; }
        public int AttendanceType { get; set; }
        public int AbsenseReason { get; set; }
        public string Description { get; set; }

        public int FirstSession { get; set; }
        public int SecondSession { get; set; }
        public int ThirdSession { get; set; }
        public int FourthSession { get; set; }
        public int FifthSession { get; set; }
        public int SixthSession { get; set; }
        public int SeventhSession { get; set; }
        public int EighthSession { get; set; }
    }
}