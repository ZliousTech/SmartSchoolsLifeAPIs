//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartSchoolLifeAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScheduledBusTrip
    {
        public int TripPassengerID { get; set; }
        public Nullable<int> SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public Nullable<int> Direction { get; set; }
        public string TripDate { get; set; }
        public string TripTime { get; set; }
        public string BusNumber { get; set; }
        public string DriverID { get; set; }
        public string DriverName { get; set; }
        public string AttendantID { get; set; }
        public string PassengerID { get; set; }
        public Nullable<int> PassengerType { get; set; }
        public string PassengerName { get; set; }
        public Nullable<double> SchoolLongitude { get; set; }
        public Nullable<double> SchoolLatitude { get; set; }
        public Nullable<int> PassengerAgeFactor { get; set; }
        public Nullable<double> PassengerLongitude { get; set; }
        public Nullable<double> PassengerLatitude { get; set; }
        public Nullable<int> PassengerOnBoard { get; set; }
        public Nullable<int> PassengeroffBoard { get; set; }
        public Nullable<System.DateTime> PassengeronBoardDateTime { get; set; }
        public Nullable<System.DateTime> PassengeroffBoardDateTime { get; set; }
        public Nullable<double> AttendantLatitude { get; set; }
        public Nullable<double> AttendantLongitude { get; set; }
        public string AttendantName { get; set; }
        public string DriverMobileNo { get; set; }
        public string AttendantMobileNo { get; set; }
        public Nullable<long> TripID { get; set; }
        public Nullable<int> TripEnded { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> AttendanceStatus { get; set; }
        public Nullable<int> PickupOrder { get; set; }
        public string SecondAttendantID { get; set; }
        public Nullable<double> SecondAttendantLongitude { get; set; }
        public Nullable<double> SecondAttendantLatitude { get; set; }
        public string SecondAttendantName { get; set; }
        public string SecondAttendantMobileNo { get; set; }
        public bool IsAbsenceByParent { get; set; }
    }
}
