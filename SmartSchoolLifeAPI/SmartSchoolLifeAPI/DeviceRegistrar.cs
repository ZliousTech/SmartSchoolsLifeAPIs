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
    
    public partial class DeviceRegistrar
    {
        public int DeviceRegistratinId { get; set; }
        public string DeviceRegistrationCode { get; set; }
        public Nullable<int> IsDeviceRegistrationActive { get; set; }
        public string OwnerID { get; set; }
        public string OwnerMobileNumber { get; set; }
        public string OwnerType { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
    }
}
