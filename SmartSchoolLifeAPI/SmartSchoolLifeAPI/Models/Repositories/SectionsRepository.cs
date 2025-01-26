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
    public class SectionsRepository : IRepository<Sections>
    {
        public IEnumerable<Sections> GetAll()
        {
            return new List<Sections>();
        }

        public Sections GetById(int id)
        {
            return new Sections();
        }

        public Sections Add(Sections entity)
        {
            return new Sections();
        }

        public void Update(Sections entity)
        {

        }

        public void Delete(int id)
        {

        }

        public dynamic GetSchoolClassSections(int schoolID, int schoolClassID)
        {
            dynamic sections = new System.Dynamic.ExpandoObject();
            string query = "SELECT SectionID, SectionCode, SectionArabicName, SectionEnglishName FROM Sections " +
                "WHERE SchoolID = @SchoolID AND SchoolClassID = @SchoolClassID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolClassID", schoolClassID);
                    comm.Parameters.AddWithValue("@SchoolID", schoolID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        sections = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return sections;
        }
    }
}