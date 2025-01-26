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
    public class SystemSettingsperSchoolRepository : IRepository<SystemSettingsperSchool>
    {
        public IEnumerable<SystemSettingsperSchool> GetAll()
        {
            return new List<SystemSettingsperSchool>();
        }

        public SystemSettingsperSchool GetById(int id)
        {
            return new SystemSettingsperSchool();
        }

        public SystemSettingsperSchool Add(SystemSettingsperSchool entity)
        {
            return new SystemSettingsperSchool();
        }

        public void Update(SystemSettingsperSchool entity)
        {

        }

        public void Delete(int id)
        {

        }

        public dynamic GetTimeTableType(int schoolID)
        {
            dynamic systemSettingsperSchool = new System.Dynamic.ExpandoObject();
            string query = "SELECT TimetableType FROM SystemSettingsperSchool " +
                "WHERE SchoolID = @SchoolID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        systemSettingsperSchool = reader.MapSingle();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return systemSettingsperSchool;
        }
    }
}