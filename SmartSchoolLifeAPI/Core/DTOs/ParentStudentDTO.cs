using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.DTOs
{
    public class ParentStudentDTO
    {
        public string PassengerID { get; set; }
        public string PassengerName { get; set; }
        public byte[] Photo { get; set; }
        public string SchoolID { get; set; }
        public string SchoolClassID { get; set; }
        public string SectionID { get; set; }
        public IEnumerable<StudentBusTrip> StudentBusTrips { get; set; }
    }
}