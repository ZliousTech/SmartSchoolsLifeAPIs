namespace SmartSchoolAPI.Entities
{
    public class StudentAttendancesRequest
    {
        public int SchoolId { get; set; }
        public int? SchoolClassId { get; set; }
        public int? SectionId { get; set; }
        public string TeacherId { get; set; }
        public string AttendanceDate { get; set; }
        public int? AttendanceType { get; set; }
    }
}