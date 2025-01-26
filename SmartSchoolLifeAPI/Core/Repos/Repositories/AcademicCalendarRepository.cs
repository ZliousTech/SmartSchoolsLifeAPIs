using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class AcademicCalendarRepository : IAcademicCalendarRepository
    {
        public IEnumerable<AcademicCalendar> GetAll()
        {
            throw new NotImplementedException();
        }

        public AcademicCalendar GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AcademicCalendar Add(AcademicCalendar entity)
        {
            throw new NotImplementedException();
        }

        public void Update(AcademicCalendar entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public dynamic GetStudentCalendar(string studentId, int schoolId)
        {
            int realStudentSchoolId = 0;
            string query = "SELECT SchoolID FROM StudentSchoolDetails WHERE StudentID = @StudentID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", studentId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            realStudentSchoolId = Convert.ToInt32(reader["SchoolID"].ToString());
                        }
                    }
                }
            }

            if (realStudentSchoolId != schoolId)
                throw new Exception($"The provided schoolId: {schoolId} doesn't match the actual Student SchoolID");


            List<dynamic> academicCalendar = GetCalendarBySchoolID(schoolId);
            List<dynamic> studentActivity = GetTeacherActivitiesByStudentId(studentId);

            dynamic result = new
            {
                AcademicCalendar = academicCalendar,
                StudentActivity = studentActivity
            };

            return academicCalendar.Any() || studentActivity.Any() ?
                result : null;
        }


        /*                           Private Methods                                */
        private List<dynamic> GetCalendarBySchoolID(int schoolId)
        {
            string query = string.Empty;
            int companyId = 0;
            List<dynamic> academicCalendar = new List<dynamic>();

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();

                query = "SELECT CompanyID FROM SchoolBranches WHERE SchoolID = @SchoolID";
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            companyId = Convert.ToInt32(reader["CompanyID"].ToString());
                        }
                    }
                }

                if (companyId != 0)
                {
                    query = "SELECT a.* FROM AcademicCalendar a " +
                                "INNER JOIN AcademicCalendars b " +
                                "ON a.CalendarID = b.CalendarID " +
                                "WHERE (YEAR(a.StartingDate) = YEAR(GETDATE())) AND " +
                                "b.SchoolID IN (1000000, " + schoolId + ") OR (b.SchoolID = -1 AND b.CompanyID = " + companyId + ") " +
                                "ORDER BY a.StartingDate";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            academicCalendar = reader.MapAll("dd/MM/yyyy");
                        }
                    }
                }
            }

            return academicCalendar;
        }

        private List<dynamic> GetTeacherActivitiesByStudentId(string studentId)
        {
            string query = string.Empty;
            List<dynamic> studentActivity = new List<dynamic>();

            query = "SELECT * FROM  TeacherActivity WHERE ID IN " +
                "(SELECT TeacherActivityID FROM TeacherStudentsActivity WHERE StudentID = @StudentID) ";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", studentId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        studentActivity = reader.MapAll("dd/MM/yyyy");
                    }
                }
            }

            return studentActivity;
        }
    }
}