using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class StaffRepository : IStaffRepository
    {
        public IEnumerable<dynamic> GetStaffData()
        {
            List<dynamic> staff = new List<dynamic>();
            string query = "SELECT sf.StaffID, sf.StaffArabicName, sf.StaffEnglishName, sf.NationalNumber, " +
                "sjd.DateOfJoining, " +
                "scd.Email, scd.MobileNo, " +
                "d.DesignationArabicText, d.DesignationEnglishText  " +
                "FROM Staff sf " +
                "INNER JOIN StaffContactDetails scd ON sf.StaffID = scd.StaffID " +
                "INNER JOIN StaffJobDetails sjd ON sf.StaffID = sjd.StaffID " +
                "INNER JOIN Designations d ON sjd.Designation = d.DesignationID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            staff.Add(new
                            {
                                StaffID = reader["StaffID"].ToString(),
                                StaffArabicName = reader["StaffArabicName"].ToString().Replace('-', ' ').Trim(),
                                StaffEnglishName = reader["StaffEnglishName"].ToString().Replace('-', ' ').Trim(),
                                NationalNumber = reader["NationalNumber"].ToString(),
                                DateOfJoining = Convert.ToDateTime(reader["DateOfJoining"]).ToString("dd/MM/yyyy"),
                                Email = reader["Email"].ToString(),
                                MobileNo = reader["MobileNo"].ToString(),
                                DesignationArabicText = reader["DesignationEnglishText"].ToString(),
                                DesignationEnglishText = reader["DesignationEnglishText"].ToString()
                            });
                        }
                    }
                }
            }

            return staff;
        }
    }
}