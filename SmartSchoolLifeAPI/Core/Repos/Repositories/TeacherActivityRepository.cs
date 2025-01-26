using SmartSchoolLifeAPI.Core.Models.Activity;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class TeacherActivityRepository : ITeacherActivityRepository
    {
        public IEnumerable<TeacherActivity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TeacherActivity GetById(int id)
        {
            object model = null;
            List<string> students = new List<string>();
            TeacherActivity result = null;

            string queryTA = "SELECT * FROM TeacherActivity WHERE ID = @ID";
            string queryTSA = "SELECT StudentID FROM TeacherStudentsActivity WHERE TeacherActivityID = @ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();

                using (SqlCommand comm = new SqlCommand(queryTA, conn))
                {
                    comm.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        model = reader.MapSingle("dd/MM/yyyy");
                    }
                }
                result = model.MapObjectTo<TeacherActivity>();

                using (SqlCommand comm = new SqlCommand(queryTSA, conn))
                {
                    comm.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(reader["StudentID"].ToString());
                        }
                    }
                }
                result.Students = students.ToList();
            }

            return result != null ? result : null;
        }

        public TeacherActivity Add(TeacherActivity entity)
        {
            int insertedActivityId = 0;

            string query = "INSERT INTO TeacherActivity " +
                "(SchoolID, SchoolYear, SchoolClassID, SectionID, Photo, " +
                "ArabicHeader, EnglishHeader, ArabicDescription, EnglishDescription, " +
                "OccasionType, StartingDate, NumberofDays, Vacation, TeacherID) " +
                "VALUES (@SchoolID, @SchoolYear, @SchoolClassID, @SectionID, @Photo, " +
                "@ArabicHeader, @EnglishHeader, @ArabicDescription, @EnglishDescription, " +
                "@OccasionType, @StartingDate, @NumberofDays, @Vacation, @TeacherID); " +
                "SELECT SCOPE_IDENTITY() AS Result;";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlCommand comm = new SqlCommand(query, conn, transaction))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", entity.SchoolID);
                        comm.Parameters.AddWithValue("@SchoolYear", entity.SchoolYear);
                        comm.Parameters.AddWithValue("@SchoolClassID", entity.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", entity.SectionID);
                        comm.Parameters.AddWithValue("@Photo", Convert.FromBase64String(entity.Photo));
                        comm.Parameters.AddWithValue("@ArabicHeader", entity.ArabicHeader);
                        comm.Parameters.AddWithValue("@EnglishHeader", entity.EnglishHeader);
                        comm.Parameters.AddWithValue("@ArabicDescription", entity.ArabicDescription);
                        comm.Parameters.AddWithValue("@EnglishDescription", entity.EnglishDescription);
                        comm.Parameters.AddWithValue("@OccasionType", 8); // for School Activities.
                        comm.Parameters.AddWithValue("@StartingDate", DateTime.ParseExact(entity.StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                        comm.Parameters.AddWithValue("@NumberofDays", entity.NumberofDays);
                        comm.Parameters.AddWithValue("@Vacation", entity.Vacation);
                        comm.Parameters.AddWithValue("@TeacherID", entity.TeacherID);

                        insertedActivityId = Convert.ToInt32(comm.ExecuteScalar());
                    }

                    StringBuilder querybuilder = new StringBuilder();
                    foreach (var student in entity.Students)
                    {
                        querybuilder.Append("INSERT INTO TeacherStudentsActivity (TeacherActivityID, StudentID) VALUES(")
                            .Append(insertedActivityId)
                            .Append(", '")
                            .Append(student)
                            .Append("')");
                    }
                    using (SqlCommand comm = new SqlCommand(querybuilder.ToString(), conn, transaction))
                    {
                        comm.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }

            return GetById(insertedActivityId);
        }

        public void Update(TeacherActivity entity)
        {
            string queryDelete = "DELETE FROM TeacherStudentsActivity WHERE TeacherActivityID = @ID";
            string queryUpdate = "UPDATE TeacherActivity SET " +
                 "SchoolID = @SchoolID, SchoolYear = @SchoolYear, SchoolClassID = @SchoolClassID, " +
                 "SectionID = @SectionID, Photo = @Photo, " +
                 "ArabicHeader = @ArabicHeader, EnglishHeader = @EnglishHeader, " +
                 "ArabicDescription = @ArabicDescription, EnglishDescription = @EnglishDescription, " +
                 "OccasionType = @OccasionType, StartingDate = @StartingDate, NumberofDays = @NumberofDays, " +
                 "Vacation = @Vacation, TeacherID = @TeacherID " +
                 "WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlCommand comm = new SqlCommand(queryDelete, conn, transaction))
                    {
                        comm.Parameters.AddWithValue("@ID", entity.ID);
                        comm.ExecuteNonQuery();
                    }

                    using (SqlCommand comm = new SqlCommand(queryUpdate, conn, transaction))
                    {
                        comm.Parameters.AddWithValue("@ID", entity.ID);
                        comm.Parameters.AddWithValue("@SchoolID", entity.SchoolID);
                        comm.Parameters.AddWithValue("@SchoolYear", entity.SchoolYear);
                        comm.Parameters.AddWithValue("@SchoolClassID", entity.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", entity.SectionID);
                        comm.Parameters.AddWithValue("@Photo", Convert.FromBase64String(entity.Photo));
                        comm.Parameters.AddWithValue("@ArabicHeader", entity.ArabicHeader);
                        comm.Parameters.AddWithValue("@EnglishHeader", entity.EnglishHeader);
                        comm.Parameters.AddWithValue("@ArabicDescription", entity.ArabicDescription);
                        comm.Parameters.AddWithValue("@EnglishDescription", entity.EnglishDescription);
                        comm.Parameters.AddWithValue("@OccasionType", 8); // for School Activities.
                        comm.Parameters.AddWithValue("@StartingDate", DateTime.ParseExact(entity.StartingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                        comm.Parameters.AddWithValue("@NumberofDays", entity.NumberofDays);
                        comm.Parameters.AddWithValue("@Vacation", entity.Vacation);
                        comm.Parameters.AddWithValue("@TeacherID", entity.TeacherID);
                        comm.ExecuteNonQuery();
                    }

                    StringBuilder queryInsert = new StringBuilder().Append(" ");
                    foreach (var student in entity.Students)
                    {
                        queryInsert.Append("INSERT INTO TeacherStudentsActivity (TeacherActivityID, StudentID) VALUES(")
                            .Append(entity.ID)
                            .Append(", '")
                            .Append(student)
                            .Append("')");
                    }
                    using (SqlCommand comm = new SqlCommand(queryInsert.ToString(), conn, transaction))
                    {
                        comm.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM TeacherActivity WHERE ID = @ID; " +
                "DELETE FROM TeacherStudentsActivity WHERE TeacherActivityID = @ID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@ID", id);
                    comm.ExecuteNonQuery();
                }
            }
        }


        public dynamic PrepareTeacherActivity(string teacherId, string schoolClassId, string sectionId, int pageNumber, int pageSize)
        {
            List<dynamic> teacherActivities = new List<dynamic>();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT t.ID, t.ArabicHeader, t.EnglishHeader, " +
                "c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "s.SectionArabicName, s.SectionEnglishName, t.NumberofDays, t.StartingDate " +
                "FROM TeacherActivity t " +
                "INNER JOIN SchoolClasses c ON t.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN Sections s ON t.SectionID = s.SectionID " +
                "WHERE t.TeacherID = @TeacherID ");
            query.Append(!string.IsNullOrEmpty(schoolClassId) ? " AND t.SchoolClassID = " + schoolClassId : "");
            query.Append(!string.IsNullOrEmpty(sectionId) ? " AND t.SectionID = " + sectionId : "");
            query.Append(" ORDER BY t.ID DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");


            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    comm.Parameters.AddWithValue("@PageSize", pageSize);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        teacherActivities = reader.MapAll("dd/MM/yyyy");
                    }
                }
            }

            return teacherActivities;
        }
    }
}