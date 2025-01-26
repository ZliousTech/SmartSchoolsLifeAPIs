using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Settings.Subject;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class SubjectsRepository : ISubjectReopsitory
    {
        public IEnumerable<SubjectModel> GetAll()
        {
            return new List<SubjectModel>();
        }

        public SubjectModel GetById(int id)
        {
            return new SubjectModel();
        }

        public SubjectModel Add(SubjectModel entity)
        {
            int insertedSubjectId = 0;
            return GetById(insertedSubjectId);
        }

        public void Update(SubjectModel entity)
        {

        }

        public void Delete(int id)
        {

        }

        // TODO: Uncomment this function.
        //public dynamic GetTeacherSubjects(string teacherId, int schoolClassId)
        //{
        //    dynamic teacherSubjects = new System.Dynamic.ExpandoObject();
        //    string query = "SELECT DISTINCT(j.SubjectID), j.SubjectArabicName, j.SubjectEnglishName FROM TimetableItems t " +
        //        "INNER JOIN Subjects j ON t.SubjectID = j.SubjectID " +
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
        //                teacherSubjects = reader.MapAll();
        //            }
        //        }
        //        conn.Close();
        //        conn.Dispose();
        //    }

        //    return teacherSubjects;
        //}


        // Hint: this is a temporary solution.
        public dynamic GetTeacherSubjects(string teacherId, int schoolClassId)
        {
            dynamic teacherSubjects = new System.Dynamic.ExpandoObject();
            string query = "SELECT s.SubjectID, s.SubjectArabicName, s.SubjectEnglishName " +
                "FROM TeacherExperiences t " +
                "INNER JOIN Subjects s ON s.SubjectID = t.SubjectID " +
                "WHERE t.TeacherID = @TeacherID AND s.SchoolClassID = @SchoolClassID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SchoolClassID", schoolClassId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        teacherSubjects = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return teacherSubjects;
        }
    }
}