using System;

namespace SmartSchoolLifeAPI.ViewModels
{
    public class BusStudentMV
    {
        public string PassengerID { get; set; }
        public string PassengerName { get; set; }
        public Nullable<double> PassengerLongitude { get; set; }
        public Nullable<double> PassengerLatitude { get; set; }
        public string TripDate { get; set; }
        public string TripTime { get; set; }
        public Nullable<int> Direction { get; set; }
        public Nullable<double> SchoolLongitude { get; set; }
        public Nullable<double> SchoolLatitude { get; set; }
        public Nullable<double> AttendantLatitude { get; set; }
        public Nullable<double> AttendantLongitude { get; set; }
        public string BusNumber { get; set; }
        public Nullable<int> PassengerOnBoard { get; set; }
        public Nullable<int> PassengerOffBoard { get; set; }
        public Nullable<int> TripEnded { get; set; }
        public Nullable<int> GuardianID { get; set; }
        public string AttendantID { get; set; }
        public string DeviceRegistrationCode { get; set; }
        public string SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public string SchoolClassID { get; set; }
        public string SectionID { get; set; }
        public byte[] Photo { get; set; }
    }
}