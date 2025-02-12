using FireBase.Service;
using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class TeacherExamRepository : ITeacherExamRepository
    {
        private readonly PushNotificationHandler _pushNotificationHandler;
        public TeacherExamRepository()
        {
            _pushNotificationHandler = new PushNotificationHandler();
        }
        public IEnumerable<Exam> GetAll()
        {
            throw new NotImplementedException();
        }

        public Exam GetById(int id)
        {
            object model = null;

            string query = "SELECT * FROM Exams WHERE ID = @ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        model = reader.MapSingle("dd/MM/yyyy");
                    }
                }
            }

            return model != null ? model.MapObjectTo<Exam>() : null;
        }

        public Exam Add(Exam entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Exam> AddAsync(Exam entity, DeviceType deviceType)
        {
            int insertedExamId = 0;

            string query = "INSERT INTO Exams " +
                "(SchoolID, SchoolYear, SchoolClassID, SectionID, SemesterID, SubjectID, " +
                "ExamTypeID, ExamTitleID, TotalGrades, TeacherID, IsCounted, ExamDate) " +
                "VALUES (@SchoolID, @SchoolYear, @SchoolClassID, @SectionID, @SemesterID, @SubjectID, " +
                "@ExamTypeID, @ExamTitleID, @TotalGrades, @TeacherID, @IsCounted, @ExamDate); " +
                "SELECT SCOPE_IDENTITY() AS Result;";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", entity.SchoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", entity.SchoolYear);
                    comm.Parameters.AddWithValue("@SchoolClassID", entity.SchoolClassID);
                    comm.Parameters.AddWithValue("@SectionID", entity.SectionID);
                    comm.Parameters.AddWithValue("@SemesterID", entity.SemesterID);
                    comm.Parameters.AddWithValue("@SubjectID", entity.SubjectID);
                    comm.Parameters.AddWithValue("@ExamTypeID", entity.ExamTypeID);
                    comm.Parameters.AddWithValue("@ExamTitleID", 5); // 5 is Quiz in ExamTitles table 👁‍🗨.
                    comm.Parameters.AddWithValue("@TotalGrades", entity.TotalGrades);
                    comm.Parameters.AddWithValue("@TeacherID", entity.TeacherID);
                    comm.Parameters.AddWithValue("@IsCounted", 0);
                    comm.Parameters.AddWithValue("@ExamDate", DateTime.ParseExact(entity.ExamDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));

                    insertedExamId = Convert.ToInt32(comm.ExecuteScalar());
                }
            }

            // Send push Notification.
            var guardians = _pushNotificationHandler.GetParentsSection(entity.SectionID);
            await _pushNotificationHandler.SendPushNotification(entity.TeacherID, entity.SchoolClassID, entity.SectionID, entity.SubjectID,
                "Exam", guardians, DateTime.ParseExact(entity.ExamDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"), deviceType);

            return GetById(insertedExamId);
        }

        public void Update(Exam entity)
        {
            string query = "UPDATE Exams SET " +
                     "SchoolID = @SchoolID, SchoolYear = @SchoolYear, SchoolClassID = @SchoolClassID, " +
                     "SectionID = @SectionID, SemesterID = @SemesterID, SubjectID = @SubjectID, " +
                     "ExamTypeID = @ExamTypeID, ExamTitleID = @ExamTitleID, TotalGrades = @TotalGrades, " +
                     "TeacherID = @TeacherID, IsCounted = @IsCounted, ExamDate = @ExamDate " +
                     "WHERE ID = @ID;";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@ID", entity.ID);
                    comm.Parameters.AddWithValue("@SchoolID", entity.SchoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", entity.SchoolYear);
                    comm.Parameters.AddWithValue("@SchoolClassID", entity.SchoolClassID);
                    comm.Parameters.AddWithValue("@SectionID", entity.SectionID);
                    comm.Parameters.AddWithValue("@SemesterID", entity.SemesterID);
                    comm.Parameters.AddWithValue("@SubjectID", entity.SubjectID);
                    comm.Parameters.AddWithValue("@ExamTypeID", entity.ExamTypeID);
                    comm.Parameters.AddWithValue("@ExamTitleID", entity.ExamTitleID);
                    comm.Parameters.AddWithValue("@TotalGrades", entity.TotalGrades);
                    comm.Parameters.AddWithValue("@TeacherID", entity.TeacherID);
                    comm.Parameters.AddWithValue("@IsCounted", 0);
                    comm.Parameters.AddWithValue("@ExamDate", DateTime.ParseExact(entity.ExamDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                    comm.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM Exams WHERE ID = @ID";

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

        //private IEnumerable<int> GetTeacherSections(string teacherId)
        //{
        //    List<int> teacherSections = new List<int>();
        //    string query = "SELECT DISTINCT(SectionID) FROM TimetableItems " +
        //        "WHERE TeacherID = @TeacherID AND SchoolYear = @SchoolYear";

        //    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
        //    {
        //        conn.Open();
        //        using (SqlCommand comm = new SqlCommand(query, conn))
        //        {
        //            comm.Parameters.AddWithValue("@TeacherID", teacherId);
        //            comm.Parameters.AddWithValue("@SchoolYear", new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear);
        //            using (SqlDataReader reader = comm.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    teacherSections.Add(Convert.ToInt32(reader["SectionID"].ToString()));
        //                }
        //            }
        //        }
        //        conn.Close();
        //        conn.Dispose();
        //    }

        //    return teacherSections;
        //}

        private IEnumerable<int> GetSections(int schoolId)
        {
            List<int> sections = new List<int>();
            string query = "SELECT DISTINCT(SectionID) FROM Sections " +
                "WHERE SchoolID = @SchoolID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sections.Add(Convert.ToInt32(reader["SectionID"].ToString()));
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return sections;
        }

        private IEnumerable<int> GetTeacherSubjects(string teacherId)
        {
            List<int> teacherSubjects = new List<int>();
            string query = "SELECT DISTINCT(SubjectID) FROM TimetableItems " +
                "WHERE TeacherID = @TeacherID AND SchoolYear = @SchoolYear";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SchoolYear", new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teacherSubjects.Add(Convert.ToInt32(reader["SubjectID"].ToString()));
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return teacherSubjects;
        }

        public dynamic GetExamsForTeacher(string teacherId, int schoolID, string semesterID,
            string schoolClassId, string sectionId, string subjectId, string examTypeId, string schoolYear, int pageNumber, int pageSize)
        {
            IEnumerable<dynamic> teacherExams = new List<dynamic>();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT e.ID, e.TotalGrades, e.ExamDate, " +
                "c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "s.SectionArabicName, s.SectionEnglishName, " +
                "m.SemesterArabicName, m.SemesterEnglishName, " +
                "j.SubjectArabicName, j.SubjectEnglishName, " +
                "t.TypeArabicName, t.TypeEnglishName, " +
                "l.ExamTitleArabicName, l.ExamTitleEnglishName " +
                "FROM Exams e " +
                "INNER JOIN SchoolClasses c ON e.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN Sections S ON e.SectionID = S.SectionID " +
                "INNER JOIN Semesters m ON e.SemesterID = m.ID " +
                "INNER JOIN Subjects j ON e.SubjectID = j.SubjectID " +
                "INNER JOIN ExamTypes t ON e.ExamTypeID = t.ExamTypeID " +
                "INNER JOIN ExamTitles l ON e.ExamTitleID = l.ID " +
                "WHERE e.SchoolID = @SchoolID AND e.SchoolYear = @SchoolYear AND " +
                "(e.TeacherID = '-1' ");

            //query.Append(!string.IsNullOrEmpty(teacherId) ? "OR e.TeacherID = '" + teacherId + "') AND " +
            //    "e.SectionID IN (" + string.Join(",", GetTeacherSections(teacherId)) + ") AND " +
            //    "e.SubjectID IN (" + string.Join(",", GetTeacherSubjects(teacherId)) + ")" : ")");

            query.Append(!string.IsNullOrEmpty(teacherId) ? "OR e.TeacherID = '" + teacherId + "') AND " +
                "e.SectionID IN (" + string.Join(",", GetSections(schoolID)) + ") AND " +
                "e.SubjectID IN (" + string.Join(",", GetTeacherSubjects(teacherId)) + ")" : ")");

            query.Append(!string.IsNullOrEmpty(schoolClassId) ? " AND e.SchoolClassID = " + schoolClassId : "");
            query.Append(!string.IsNullOrEmpty(sectionId) ? " AND e.SectionID = " + sectionId : "");
            query.Append(!string.IsNullOrEmpty(subjectId) ? " AND e.SubjectID = " + subjectId : "");
            query.Append(!string.IsNullOrEmpty(semesterID) ? " AND e.SemesterID = " + semesterID : "");
            query.Append(!string.IsNullOrEmpty(examTypeId) ? " AND e.ExamTypeID = " + examTypeId : "");
            query.Append(" ORDER BY e.ID DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", !string.IsNullOrEmpty(schoolYear) ?
                        schoolYear : new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear);
                    comm.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    comm.Parameters.AddWithValue("@PageSize", pageSize);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        teacherExams = reader.MapAll("dd/MM/yyyy");
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return teacherExams;
        }

        public void AddOrUpdateExamsDates(Dictionary<int, string> examsDates)
        {
            StringBuilder query = new StringBuilder();
            foreach (var kvp in examsDates)
            {
                query.Append("UPDATE Exams SET ExamDate = '")
                    .Append(DateTime.ParseExact(kvp.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"))
                    .Append("' WHERE ID = ")
                    .Append(kvp.Key).Append(";");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }

    }
}