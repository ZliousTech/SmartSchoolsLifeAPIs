using System;

namespace SmartSchoolLifeAPI.ViewModels
{
    public class BusStudentVM
    {
        public string TripTime { get; set; }
        public Nullable<int> Direction { get; set; }
        public string BusNumber { get; set; }
        public string SchoolYear { get; set; }
    }
}