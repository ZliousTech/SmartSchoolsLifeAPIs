using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.HomeWork;
using SmartSchoolLifeAPI.Core.Models.Shared;
using SmartSchoolLifeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class HomeWorkRepository : IHomeWorkRepository
    {
        private readonly PushNotificationHandler _pushNotificationHandler;
        public HomeWorkRepository()
        {
            _pushNotificationHandler = new PushNotificationHandler();
        }

        public IEnumerable<HomeWorkModel> GetAll()
        {
            return new List<HomeWorkModel>();
        }

        public HomeWorkModel GetById(int id)
        {
            object model = null;

            string query = "SELECT HomeWorkID, SchoolClassID, SectionID, SubjectID, HomeWorkTitle, " +
                "HomeWorkCreationDate, HomeWorkDeadLine, HomeWorkAttachment, [.Ext] AS Ext, " +
                "HomeWorkNote, TeacherID " +
                "FROM HomeWork WHERE HomeWorkID = @HomeWorkID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@HomeWorkID", id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        model = reader.MapSingle("dd/MM/yyyy");
                    }
                }
            }

            return model != null ? model.MapObjectTo<HomeWorkModel>() : null;
        }

        public HomeWorkModel Add(HomeWorkModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<HomeWorkModel> AddAsync(HomeWorkModel entity)
        {
            int insertedHomeworkId = 0;

            if (!string.IsNullOrEmpty(entity.HomeWorkAttachment))
            {
                string query = "INSERT INTO HomeWork " +
                    "(SchoolClassID, SectionID, SubjectID, HomeWorkTitle, HomeWorkDeadLine, " +
                    "HomeWorkAttachment, [.Ext], HomeWorkNote, TeacherID) " +
                    "VALUES (@SchoolClassID, @SectionID, @SubjectID, @HomeWorkTitle, " +
                    "@HomeWorkDeadLine, @HomeWorkAttachment, @Ext, @HomeWorkNote, @TeacherID); " +
                    "SELECT SCOPE_IDENTITY() AS Result;";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolClassID", entity.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", entity.SectionID);
                        comm.Parameters.AddWithValue("@SubjectID", entity.SubjectID);
                        comm.Parameters.AddWithValue("@HomeWorkTitle", entity.HomeWorkTitle);
                        comm.Parameters.AddWithValue("@HomeWorkDeadLine", DateTime.ParseExact(entity.HomeWorkDeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                        comm.Parameters.AddWithValue("@HomeWorkAttachment", entity.HomeWorkAttachment);
                        comm.Parameters.AddWithValue("@TeacherID", entity.TeacherID);
                        comm.Parameters.AddWithValue("@Ext", entity.Ext);
                        if (!string.IsNullOrEmpty(entity.HomeWorkNote))
                            comm.Parameters.AddWithValue("@HomeWorkNote", entity.HomeWorkNote);
                        else
                            comm.Parameters.AddWithValue("@HomeWorkNote", DBNull.Value);

                        insertedHomeworkId = Convert.ToInt32(comm.ExecuteScalar());
                    }
                }
            }

            // Send push Notification.
            var guardians = _pushNotificationHandler.GetParentsSection(entity.SectionID);
            await _pushNotificationHandler.SendPushNotification(entity.TeacherID, entity.SchoolClassID, entity.SectionID, entity.SubjectID,
                "Homework", guardians);

            return GetById(insertedHomeworkId);
        }

        public void Update(HomeWorkModel entity)
        {
            string query = string.Empty;
            if (string.IsNullOrEmpty(entity.HomeWorkAttachment) && string.IsNullOrEmpty(entity.Ext))
            {
                query = "UPDATE HomeWork SET " +
                "SchoolClassID = @SchoolClassID, SectionID = @SectionID, SubjectID = @SubjectID, " +
                "HomeWorkTitle = @HomeWorkTitle, HomeWorkDeadLine = @HomeWorkDeadLine, " +
                "HomeWorkNote = @HomeWorkNote, TeacherID = @TeacherID " +
                "WHERE HomeWorkID = @HomeWorkID";
            }
            else
            {
                query = "UPDATE HomeWork SET " +
                "SchoolClassID = @SchoolClassID, SectionID = @SectionID, SubjectID = @SubjectID, " +
                "HomeWorkTitle = @HomeWorkTitle, HomeWorkDeadLine = @HomeWorkDeadLine, " +
                "HomeWorkAttachment = @HomeWorkAttachment, [.Ext] = @Ext, " +
                "HomeWorkNote = @HomeWorkNote, TeacherID = @TeacherID " +
                "WHERE HomeWorkID = @HomeWorkID";
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolClassID", entity.SchoolClassID);
                    comm.Parameters.AddWithValue("@SectionID", entity.SectionID);
                    comm.Parameters.AddWithValue("@SubjectID", entity.SubjectID);
                    comm.Parameters.AddWithValue("@HomeWorkTitle", entity.HomeWorkTitle);
                    comm.Parameters.AddWithValue("@HomeWorkDeadLine", DateTime.ParseExact(entity.HomeWorkDeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                    if (!string.IsNullOrEmpty(entity.HomeWorkAttachment) && !string.IsNullOrEmpty(entity.Ext))
                    {
                        comm.Parameters.AddWithValue("@HomeWorkAttachment",
                                        !string.IsNullOrEmpty(entity.HomeWorkAttachment) ? entity.HomeWorkAttachment : (object)DBNull.Value);
                        comm.Parameters.AddWithValue("@Ext", !string.IsNullOrEmpty(entity.Ext) ?
                            entity.Ext : (object)DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(entity.HomeWorkNote))
                        comm.Parameters.AddWithValue("@HomeWorkNote", entity.HomeWorkNote);
                    else
                        comm.Parameters.AddWithValue("@HomeWorkNote", DBNull.Value);
                    comm.Parameters.AddWithValue("@TeacherID", entity.TeacherID);
                    comm.Parameters.AddWithValue("@HomeWorkID", entity.HomeWorkID);

                    comm.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM HomeWork WHERE HomeWorkID = @HomeWorkID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@HomeWorkID", id);
                    comm.ExecuteNonQuery();
                }
            }
        }

        public HomeWorkVM GetViewById(int id)
        {
            object model = null;

            string query = "SELECT h.HomeWorkID, h.SchoolClassID, h.SectionID, h.SubjectID, " +
                "h.HomeWorkTitle, h.HomeWorkCreationDate, h.HomeWorkDeadLine, h.HomeWorkAttachment, " +
                "h.[.Ext] AS Ext, h.HomeWorkNote, h.TeacherID, " +
                "c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "s.SectionArabicName, s.SectionEnglishName, " +
                "j.SubjectArabicName, j.SubjectEnglishName, " +
                "f.StaffArabicName AS TeacherArabicName, f.StaffEnglishName AS TeacherEnglishName " +
                "FROM HomeWork h " +
                "INNER JOIN SchoolClasses c ON h.SchoolClassID = h.SchoolClassID " +
                "INNER JOIN Sections s ON h.SectionID = s.SectionID " +
                "INNER JOIN Subjects j ON h.SubjectID = j.SubjectID " +
                "INNER JOIN Staff f ON h.TeacherID = f.StaffID " +
                "WHERE h.HomeWorkID = @HomeWorkID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@HomeWorkID", id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        model = reader.MapSingle();
                    }
                }
            }

            return model != null ? model.MapObjectTo<HomeWorkVM>() : null;
        }

        public IEnumerable<dynamic> GetStudentHomeWork(int sectionId, int pageNumber, int pageSize)
        {
            List<dynamic> homeworksList = new List<dynamic>();
            string query = @"
                SELECT h.HomeWorkID, h.SchoolClassID, h.SectionID, h.SubjectID, h.HomeWorkTitle, 
                       h.HomeWorkCreationDate, h.HomeWorkDeadLine, h.HomeWorkNote, 
                       h.TeacherID, j.SubjectArabicName, j.SubjectEnglishName, 
                       c.SchoolClassArabicName, c.SchoolClassEnglishName, 
                       s.SectionArabicName, s.SectionEnglishName, t.StaffArabicName, t.StaffEnglishName
                FROM HomeWork h
                INNER JOIN SchoolClasses c ON h.SchoolClassID = c.SchoolClassID
                INNER JOIN Sections s ON h.SectionID = s.SectionID
                INNER JOIN Staff t ON h.TeacherID = t.StaffID
                INNER JOIN Subjects j ON h.SubjectID = j.SubjectID
                WHERE h.SectionID = @SectionID
                ORDER BY HomeWorkID DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SectionID", sectionId);
                    comm.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    comm.Parameters.AddWithValue("@PageSize", pageSize);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        homeworksList = reader.MapAll("dd/MM/yyyy");
                    }
                }
            }

            return homeworksList;
        }


        public IEnumerable<dynamic> GetTeacherHomeWorks(string teacherId, string schoolClassId, string sectionId, string subjectId,
            int pageNumber, int pageSize)
        {
            List<dynamic> teacherHomeWorksList = new List<dynamic>();
            string query = @"
                SELECT h.HomeWorkID, h.SchoolClassID, h.SectionID, h.SubjectID, h.HomeWorkTitle, 
                       h.HomeWorkCreationDate, h.HomeWorkDeadLine, h.HomeWorkNote, 
                       h.TeacherID, c.SchoolClassArabicName, c.SchoolClassEnglishName, 
                       s.SectionArabicName, s.SectionEnglishName, 
                       j.SubjectArabicName, j.SubjectEnglishName
                FROM HomeWork h
                INNER JOIN SchoolClasses c ON h.SchoolClassID = c.SchoolClassID
                INNER JOIN Sections s ON h.SectionID = s.SectionID
                INNER JOIN Subjects j ON h.SubjectID = j.SubjectID
                WHERE h.TeacherID = @TeacherID ";

            query += !string.IsNullOrEmpty(schoolClassId) ? " AND h.SchoolClassID = @SchoolClassID " : "";
            query += !string.IsNullOrEmpty(sectionId) ? " AND h.SectionID = @SectionID " : "";
            query += !string.IsNullOrEmpty(subjectId) ? " AND h.SubjectID = @SubjectID " : "";

            query += "ORDER BY HomeWorkID DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    comm.Parameters.AddWithValue("@PageSize", pageSize);

                    if (!string.IsNullOrEmpty(schoolClassId))
                        comm.Parameters.AddWithValue("@SchoolClassID", schoolClassId);
                    if (!string.IsNullOrEmpty(sectionId))
                        comm.Parameters.AddWithValue("@SectionID", sectionId);
                    if (!string.IsNullOrEmpty(subjectId))
                        comm.Parameters.AddWithValue("@SubjectID", subjectId);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        teacherHomeWorksList = reader.MapAll("dd/MM/yyyy");
                    }
                }
            }

            return teacherHomeWorksList;
        }


        public dynamic GetAttachmentByHomeworkId(int homeworkId)
        {
            dynamic homeworkAttachment = null;

            string query = "SELECT HomeWorkAttachment, [.Ext] AS Ext " +
                "FROM HomeWork WHERE HomeWorkID = @HomeworkID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@HomeworkID", homeworkId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        homeworkAttachment = reader.MapSingle("dd/MM/yyyy");
                    }
                }
            }

            return homeworkAttachment;
        }
    }
}