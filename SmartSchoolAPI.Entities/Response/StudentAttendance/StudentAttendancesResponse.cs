namespace SmartSchoolAPI.Entities
{
    public class StudentAttendancesResponse
    {
        public string StudentId { get; set; }
        public string StudentArabicName { get; set; }
        public string StudentEnglishName { get; set; }
        public string StudentArabicClass { get; set; }
        public string StudentEnglishClass { get; set; }
        public int SectionId { get; set; }
        public string StudentArabicSection { get; set; }
        public string StudentEnglishSection { get; set; }
        public int StudentTotalAbsence { get; set; }
        public int StudenTotalPartialAttendace { get; set; }

        /* Indicates whether the student should be marked as absent (checkbox state) in PrepareAttendance view */
        public bool IsAbsence { get; set; }

        /* Indicates that an attendance record was added manually for this student on the selected date */
        public bool IsManualAttendance { get; set; }
    }
}