using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartSchoolLifeAPI.Models.Shared;
using SmartSchoolLifeAPI.Models;
using System.Data.SqlClient;
using SmartSchoolLifeAPI.Models.Extensions;
using System.Security.AccessControl;

namespace SmartSchoolLifeAPI.Models.Repositories
{
    public class SchoolSettingsRepository : IRepository<SchoolSettings>
    {
        public IEnumerable<SchoolSettings> GetAll()
        {
            return new List<SchoolSettings>();
        }

        public SchoolSettings GetById(int id)
        {
            object schoolSettings = null;
            string query = "SELECT * FROM SchoolSettings " +
                "WHERE SchoolID = @SchoolID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        schoolSettings = reader.MapSingle("HH:mm");
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return schoolSettings.MapObjectTo<SchoolSettings>();
        }

        public SchoolSettings Add(SchoolSettings entity)
        {
            return new SchoolSettings();
        }

        public void Update(SchoolSettings entity)
        {
            
        }

        public void Delete(int id)
        {

        }

        public SchoolSettings GetStudentSchoolSettings(string studentID)
        {
            dynamic schoolSettings = new System.Dynamic.ExpandoObject();
            string query = "SELECT s.* FROM SchoolSettings s " +
                "INNER JOIN StudentSchoolDetails d ON s.SchoolID = d.SchoolID " +
                "WHERE d.StudentID = @StudentID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", studentID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        schoolSettings = reader.MapSingle();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return schoolSettings.MapObjectTo<SchoolSettings>();
        }
    }
}