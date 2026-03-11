using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class SchoolClassesRepository : ISchoolClassesRepository
    {
        public IEnumerable<SchoolClasses> GetAll()
        {
            return new List<SchoolClasses>();
        }

        public SchoolClasses GetById(int id)
        {
            return new SchoolClasses();
        }

        public SchoolClasses Add(SchoolClasses entity)
        {
            return new SchoolClasses();
        }

        public void Update(SchoolClasses entity)
        {
        }

        public void Delete(int id)
        {
        }

        public dynamic GetSchoolClasses(int schoolId)
        {
            dynamic schoolClasses = new ExpandoObject();
            string query = "SELECT SchoolClassID, SchoolClassArabicName, SchoolClassEnglishName FROM SchoolClasses " +
                "WHERE SchoolID = @SchoolID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        schoolClasses = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return schoolClasses;
        }
    }
}