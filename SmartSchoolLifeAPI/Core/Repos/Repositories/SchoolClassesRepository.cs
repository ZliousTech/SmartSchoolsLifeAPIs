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

        // TODO: Uncomment this function.
        //public dynamic GetTeacherSchoolClasses(string teacherId, string schoolYear)
        //{
        //    schoolYear = !string.IsNullOrEmpty(schoolYear) ? schoolYear : new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear;
        //    dynamic teacherSchoolClasses = new ExpandoObject();
        //    string query = "SELECT DISTINCT(c.SchoolClassID), c.SchoolClassArabicName, c.SchoolClassEnglishName FROM TimetableItems t " +
        //        "INNER JOIN SchoolClasses c ON t.SchoolClassID = c.SchoolClassID " +
        //        "WHERE t.TeacherID = @TeacherID AND t.SchoolYear = @SchoolYear";

        //    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
        //    {
        //        conn.Open();
        //        using (SqlCommand comm = new SqlCommand(query, conn))
        //        {
        //            comm.Parameters.AddWithValue("@TeacherID", teacherId);
        //            comm.Parameters.AddWithValue("@SchoolYear", schoolYear);
        //            using (SqlDataReader reader = comm.ExecuteReader())
        //            {
        //                teacherSchoolClasses = reader.MapAll();
        //            }
        //        }
        //        conn.Close();
        //        conn.Dispose();
        //    }

        //    return teacherSchoolClasses;
        //}


        // Hint: this is a temporary solution.
        public dynamic GetTeacherSchoolClasses(string teacherId, string schoolYear)
        {
            schoolYear = !string.IsNullOrEmpty(schoolYear) ? schoolYear : new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear;
            dynamic teacherSchoolClasses = new ExpandoObject();
            string query = "SELECT c.SchoolClassID, c.SchoolClassArabicName, c.SchoolClassEnglishName FROM SchoolClasses c " +
                "WHERE SchoolID = " +
                "(SELECT TOP(1) t.SchoolID FROM TimetableItems t " +
                "WHERE t.TeacherID = @TeacherID AND t.SchoolYear = @SchoolYear)";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SchoolYear", schoolYear);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        teacherSchoolClasses = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return teacherSchoolClasses;
        }
    }
}