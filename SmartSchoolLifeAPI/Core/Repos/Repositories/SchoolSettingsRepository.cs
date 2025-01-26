using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class SchoolSettingsRepository : ISchoolSettingsRepository
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

            return schoolSettings != null ? schoolSettings.MapObjectTo<SchoolSettings>() : null;
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

        public static dynamic GetStudentSchoolSettings(string studentID)
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

            return schoolSettings;
        }
    }
}