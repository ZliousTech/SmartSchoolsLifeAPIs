using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class StudentRepository : IStudentRepository
    {
        public IEnumerable<dynamic> GetStudentsData()
        {
            List<dynamic> students = new List<dynamic>();
            string query = "SELECT StudentID, StudentArabicName, StudentEnglishName, NationalNumber, DateofBirth " +
                "FROM Student";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new
                            {
                                StaffID = reader["StudentID"].ToString(),
                                StaffArabicName = reader["StudentArabicName"].ToString().Replace('-', ' ').Trim(),
                                StaffEnglishName = reader["StudentEnglishName"].ToString().Replace('-', ' ').Trim(),
                                NationalNumber = reader["NationalNumber"].ToString(),
                                DateofBirth = Convert.ToDateTime(reader["DateofBirth"]).ToString("dd/MM/yyyy"),
                                Email = "student@gmail.com",
                                MobileNo = "00000000000",
                                DesignationArabicText = "طالب",
                                DesignationEnglishText = "Student"
                            });
                        }
                    }
                }
            }

            return students;
        }
    }
}