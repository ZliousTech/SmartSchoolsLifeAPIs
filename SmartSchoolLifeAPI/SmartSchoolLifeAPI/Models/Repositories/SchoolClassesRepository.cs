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
    public class SchoolClassesRepository : IRepository<SchoolClasses>
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

        public dynamic GetSchoolClasses(int schoolID)
        {
            dynamic schoolClasses = new System.Dynamic.ExpandoObject();
            string query = "SELECT SchoolClassID, SchoolClassArabicName, SchoolClassEnglishName FROM SchoolClasses " +
                "WHERE SchoolID = @SchoolID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolID);
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