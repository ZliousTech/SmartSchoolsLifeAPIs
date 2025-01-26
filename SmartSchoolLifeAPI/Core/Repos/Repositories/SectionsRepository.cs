using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class SectionsRepository : ISectionsRepository
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

        // TODO: Uncomment this function.
        //public dynamic GetTeacherSections(string teacherId, int schoolClassId)
        //{
        //    dynamic teacherSections = new System.Dynamic.ExpandoObject();
        //    string query = "SELECT DISTINCT(s.SectionID), s.SectionArabicName, s.SectionEnglishName FROM TimetableItems t " +
        //        "INNER JOIN Sections s ON t.SectionID = s.SectionID " +
        //        "WHERE t.TeacherID = @TeacherID AND t.SchoolClassID = @SchoolClassID";

        //    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
        //    {
        //        conn.Open();
        //        using (SqlCommand comm = new SqlCommand(query, conn))
        //        {
        //            comm.Parameters.AddWithValue("@TeacherID", teacherId);
        //            comm.Parameters.AddWithValue("@SchoolClassID", schoolClassId);
        //            using (SqlDataReader reader = comm.ExecuteReader())
        //            {
        //                teacherSections = reader.MapAll();
        //            }
        //        }
        //        conn.Close();
        //        conn.Dispose();
        //    }

        //    return teacherSections;
        //}


        // Hint: this is a temporary solution.
        public dynamic GetTeacherSections(string teacherId, int schoolClassId)
        {
            dynamic teacherSections = new System.Dynamic.ExpandoObject();
            string query = "SELECT s.SectionID, s.SectionArabicName, s.SectionEnglishName FROM Sections s " +
                "WHERE s.SchoolClassID = @SchoolClassID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SchoolClassID", schoolClassId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        teacherSections = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return teacherSections;
        }

        public dynamic GetSectionStudents(int sectionId)
        {
            List<dynamic> sectionStudents = new List<dynamic>();
            string query = "SELECT a.StudentID, " +
                    "a.StudentEnglishName, a.StudentArabicName " +
                    "FROM Student a " +
                    "INNER JOIN StudentSchoolDetails b " +
                    "ON a.StudentID = b.StudentID " +
                    "WHERE b.SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SectionID", sectionId);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        sectionStudents = reader.MapAll();
                    }
                }
            }

            return sectionStudents;
        }
    }
}