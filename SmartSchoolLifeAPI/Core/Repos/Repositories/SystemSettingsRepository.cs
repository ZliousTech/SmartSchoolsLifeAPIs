using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class SystemSettingsRepository : ISystemSettingsRepository
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

            return systemSettings != null ? systemSettings.MapObjectTo<SystemSettings>() : null;
        }
    }
}