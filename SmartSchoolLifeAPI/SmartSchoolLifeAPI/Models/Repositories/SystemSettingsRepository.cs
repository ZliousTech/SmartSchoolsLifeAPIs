using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartSchoolLifeAPI.Models.Shared;
using SmartSchoolLifeAPI.Models;
using System.Data.SqlClient;
using SmartSchoolLifeAPI.Models.Extensions;
using System.Security.AccessControl;
using System.Data.Entity;

namespace SmartSchoolLifeAPI.Models.Repositories
{
    public class SystemSettingsRepository
    {
        public SystemSettings GetSystemSettings()
        {
            object systemSettings = null;
            string query = "SELECT * FROM SystemSettings";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        systemSettings = reader.MapSingle();
                    }
                }
                conn.Close();
            }

            return systemSettings.MapObjectTo<SystemSettings>();
        }
    }
}