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
    
    public partial class User
    {
        public int UserID { get; set; }
        public string StaffID { get; set; }
        public string UserName { get; set; }
        public Nullable<int> UserType { get; set; }
        public string Password { get; set; }
        public Nullable<int> SchoolID { get; set; }
        public Nullable<int> CompanyID { get; set; }
    }

    public partial class UserPassword
    {
        public string UserName { get; set; }
        public int CountryKeyCode { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }

    public partial class UserPlainPassword
    {
        public string Password { get; set; }
    }
}
