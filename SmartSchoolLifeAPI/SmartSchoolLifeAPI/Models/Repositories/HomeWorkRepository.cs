using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartSchoolLifeAPI.Models.HomeWork;
using SmartSchoolLifeAPI.Models.Shared;
using SmartSchoolLifeAPI.Models.Helpers;
using SmartSchoolLifeAPI.Models.Extensions;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Models.Repositories
{
    public class HomeWorkRepository : IRepository<HomeWorkModel>
    {
        public IEnumerable<HomeWorkModel> GetAll()
        {
            return new List<HomeWorkModel>();
        }

        public HomeWorkModel GetById(int id)
        {
            return new HomeWorkModel();
        }

        public HomeWorkModel Add(HomeWorkModel entity)
        {
            return new HomeWorkModel();
        }

        public void Update(HomeWorkModel entity)
        {

        }

        public void Delete(int id)
        {

        }

        public IEnumerable<dynamic> GetStudentHomeWork(int sectionID)
        {

            List<dynamic> homeworksList = new List<dynamic>();
            string query = "SELECT h.*, c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "s.SectionArabicName, s.SectionEnglishName, t.StaffArabicName, t.StaffEnglishName " +
                "FROM HomeWork h " +
                "INNER JOIN SchoolClasses c ON h.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN Sections s ON h.SectionID = s.SectionID " +
                "INNER JOIN Staff t ON h.TeacherID = t.StaffID " +
                "WHERE h.SectionID = @SectinID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SectinID", sectionID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        homeworksList = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return homeworksList;
        }
    }
}