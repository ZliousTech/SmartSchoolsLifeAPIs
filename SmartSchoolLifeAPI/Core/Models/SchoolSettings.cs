using System.ComponentModel.DataAnnotations;

namespace SmartSchoolLifeAPI.Core.Models
{
    public class SchoolSettings
    {
        [Key]
        public int SchoolID { get; set; }
        public int NumberofClassRooms { get; set; }
        public int NumberofChairsPerClass { get; set; }
        public int NumberofSessionsPerDay { get; set; }
        public int NumberofSessionsPerTeacher { get; set; }
        public int AllowedNumberofStudentsPerYardArea { get; set; }
        public int AllowedNumberofStudentsPerClassroomArea { get; set; }
        public int WeekStartingDay { get; set; }
        public string StartingTime { get; set; }
        public string FirstClassStartingTime { get; set; }
        public int SessionDuration { get; set; }
        public int BreakBetweenSessionsDuration { get; set; }
        public int BreakDuration { get; set; }
        public int NumberofSemesters { get; set; }
        public int NumberofExamsPerSemester { get; set; }
        public int MultiDependantsDiscount { get; set; }
        public int BreakAfterSession { get; set; }
    }
}
