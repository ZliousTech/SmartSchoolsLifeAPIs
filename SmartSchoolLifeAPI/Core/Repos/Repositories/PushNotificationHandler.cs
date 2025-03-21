using SmartSchool.FireBase.Service;
using SmartSchool.FireBase.Service.DataService;
using SmartSchool.Logger.Service;
using SmartSchoolLifeAPI.Core.DTOs;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartSchoolLifeAPI.Core.Models.Shared.clsenumration;


namespace SmartSchoolLifeAPI.Core.Repos
{
    public class PushNotificationHandler : IPushNotificationHandler
    {
        private readonly IFireBaseService _fireBaseService;
        private readonly ILoggerService _loggerService;

        public PushNotificationHandler()
        {
            _fireBaseService = new FireBaseService();
            _loggerService = new LoggerService();
        }

        public async Task SendPushNotification(string staffId, int schoolClassId, int sectionId, int subjectId,
            string type, IEnumerable<dynamic> guardians, string date = "", DeviceType deviceType = DeviceType.Android)
        {
            try
            {
                int notificationTypeId = GetNotificaitonTypeId(type);
                string notificationText = NotificationBuilder(staffId, schoolClassId, sectionId, subjectId, type, date);

                if (!guardians.Any())
                {
                    #region Warning For No Sending Push Notification.

                    StringBuilder logInfoBuilder = new StringBuilder();
                    logInfoBuilder.AppendLine("Warning: No Patents found to send notification to them!");
                    logInfoBuilder.AppendLine($"Notification Type: {type}");
                    logInfoBuilder.AppendLine($"Message: {notificationText}");
                    _loggerService.Warning(nameof(SendPushNotification), logInfoBuilder.ToString());

                    #endregion

                    return;
                }
                if (notificationTypeId == -1)
                    throw new Exception("Not allowed Notification Type!");
                if (notificationText.Equals("Fail Get data for notification!"))
                    throw new Exception(notificationText);

                AddPushNotificationToDB(guardians, notificationTypeId, notificationText);

                await SendToParents(guardians, type, notificationText, string.Empty, deviceType);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SendPushNotification(string staffId, string type, IEnumerable<AttendanceInsertDTO> students, DeviceType deviceType = DeviceType.Android)
        {
            try
            {
                var studentsIds = students.Where(s => s.ParentInformed == 1).Select(s => s.StudentId).ToList();
                var teacherInfo = GetTeacherName(staffId);
                IEnumerable<dynamic> guardians = GetStudentsParents(studentsIds);
                int notificationTypeId = GetNotificaitonTypeId(type);
                string notificationText = string.Empty;

                if (!guardians.Any())
                    throw new Exception("No Patents found to send notification to them!");
                if (notificationTypeId == -1)
                    throw new Exception("Not allowed Notification Type!");

                foreach (var guardian in guardians)
                {
                    notificationText = $"{guardian.studentEnglishName} \n is absent by {teacherInfo?.StaffEnglishName}";
                    AddPushNotificationToDB(guardian, notificationTypeId, notificationText);
                    await SendToParent(guardian, type, notificationText, string.Empty, deviceType);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SendPushNotification(string studentId, IEnumerable<int> sessions, Dictionary<int, string> sessionProperties,
            string type, string date, DeviceType deviceType = DeviceType.Android)
        {
            try
            {
                IEnumerable<dynamic> guardians = GetStudentsParents(new List<string> { studentId });
                int notificationTypeId = GetNotificaitonTypeId(type);
                string notificationText = string.Empty;

                if (!guardians.Any())
                    throw new Exception("No Patent found to send notification to him!");
                if (notificationTypeId == -1)
                    throw new Exception("Not allowed Notification Type!");

                var sessionNames = sessions.OrderBy(s => s).Where(s => sessionProperties.ContainsKey(s))
                                       .Select(s => sessionProperties[s])
                                       .Select(name => name.Insert(name.IndexOf("Session"), " "))
                                       .ToList();

                string sessionList = string.Empty;
                if (sessionNames.Count > 1)
                {
                    sessionList = string.Join(", ", sessionNames.Take(sessionNames.Count - 1))
                                  + " and "
                                  + sessionNames.Last();
                }
                else if (sessionNames.Count == 1)
                {
                    sessionList = sessionNames.First();
                }

                foreach (var guardian in guardians)
                {
                    notificationText = $"{guardian.studentEnglishName} \nis absent in {sessionList} by you in \n{date}";
                    AddPushNotificationToDB(guardian, notificationTypeId, notificationText);
                    await SendToParent(guardian, type, notificationText, string.Empty, deviceType);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<dynamic> GetParentsSection(int sectionId)
        {
            List<dynamic> guardians = new List<dynamic>();

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                string query = "SELECT DeviceRegistrationCode, OwnerID FROM DeviceRegistrar WHERE OwnerID " +
                    "IN(SELECT DISTINCT CAST(GuardianID AS NVARCHAR(6)) FROM StudentGuardDetails WHERE StudentID " +
                    "IN (SELECT DISTINCT(StudentID) FROM StudentSchoolDetails WHERE SectionID = @SectionID)) " +
                    "AND DeviceRegistrationCode IS NOT NULL AND DeviceRegistrationCode <> 'null'";
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SectionID", sectionId);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guardians.Add(new
                            {
                                guardiantoken = reader[0].ToString(),
                                guardianID = reader[1].ToString()
                            });
                        }
                    }
                }
            }

            return guardians;
        }

        public IEnumerable<dynamic> GetStudentsParents(IEnumerable<string> students)
        {
            List<dynamic> guardians = new List<dynamic>();
            var studentsIds = string.Join(",", students);

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                string query = "SELECT s.StudentEnglishName, s.StudentArabicName, dr.DeviceRegistrationCode, dr.OwnerID " +
                    "FROM DeviceRegistrar dr " +
                    "INNER JOIN Student s ON CAST(s.GuardianID AS NVARCHAR(6)) = dr.OwnerID " +
                    "WHERE OwnerID IN " +
                    "(SELECT CAST(GuardianID AS NVARCHAR(6)) " +
                    "FROM StudentGuardDetails " +
                    "WHERE s.StudentID IN (" + string.Join(",", students.Select(id => $"'{id}'")) + "))";
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guardians.Add(new
                            {
                                studentEnglishName = reader["StudentEnglishName"].ToString(),
                                studentArabicName = reader["StudentArabicName"].ToString(),
                                guardiantoken = reader["DeviceRegistrationCode"].ToString(),
                                guardianID = reader["OwnerID"].ToString()
                            });
                        }
                    }
                }
            }

            return guardians;
        }

        /* Private Methods */

        private void AddPushNotificationToDB(IEnumerable<dynamic> guardians, int notificationTypeId, string notificationText)
        {
            StringBuilder queryBuilder = new StringBuilder();

            foreach (var guardian in guardians)
            {
                queryBuilder.Append("INSERT INTO Notifications " +
                    "(DesitinationID, DestinationType, NotificationTypeID, NotificationText, SourceType, [Status]) " +
                    "VALUES ('" + guardian.guardianID + "', 19, " + notificationTypeId + ", N'" + notificationText + "', 15, 2);");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(queryBuilder.ToString(), conn))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }

        private void AddPushNotificationToDB(dynamic guardian, int notificationTypeId, string notificationText)
        {
            string query = "INSERT INTO Notifications " +
                    "(DesitinationID, DestinationType, NotificationTypeID, NotificationText, SourceType, [Status]) " +
                    "VALUES ('" + guardian.guardianID + "', 19, " + notificationTypeId + ", N'" + notificationText + "', 15, 2);";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }

        private string NotificationBuilder(string staffID, int schoolClassID, int sectionID, int subjectID, string type, string date)
        {
            try
            {
                List<dynamic> informations = new List<dynamic>();

                string query = "SELECT StaffArabicName, StaffEnglishName FROM Staff " +
                               "WHERE StaffID = @StaffID; ";
                query += "SELECT SchoolClassArabicName, SchoolClassEnglishName FROM SchoolClasses " +
                         "WHERE SchoolClassID = @SchoolClassID; ";
                query += "SELECT SectionArabicName, SectionEnglishName FROM Sections " +
                         "WHERE SectionID = @SectionID; ";
                query += "SELECT SubjectArabicName, SubjectEnglishName FROM Subjects " +
                         "WHERE SubjectID = @SubjectID; ";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StaffID", staffID);
                        comm.Parameters.AddWithValue("@SchoolClassID", schoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", sectionID);
                        comm.Parameters.AddWithValue("@SubjectID", subjectID);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    StaffArabicName = reader["StaffArabicName"].ToString(),
                                    StaffEnglishName = reader["StaffEnglishName"].ToString()
                                });
                            }

                            reader.NextResult();

                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    SchoolClassArabicName = reader["SchoolClassArabicName"].ToString(),
                                    SchoolClassEnglishName = reader["SchoolClassEnglishName"].ToString()
                                });
                            }

                            reader.NextResult();

                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    SectionArabicName = reader["SectionArabicName"].ToString(),
                                    SectionEnglishName = reader["SectionEnglishName"].ToString()
                                });
                            }

                            reader.NextResult();

                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    SubjectArabicName = reader["SubjectArabicName"].ToString(),
                                    SubjectEnglishName = reader["SubjectEnglishName"].ToString()
                                });
                            }
                        }
                    }
                }

                var teacherArabicName = informations[0].StaffArabicName;
                var teacherEnglishName = informations[0].StaffEnglishName;
                var schoolClassArabicName = informations[1].SchoolClassArabicName;
                var schoolClassEnglishName = informations[1].SchoolClassEnglishName;
                var sectionArabicName = informations[2].SectionArabicName;
                var sectionEnglishName = informations[2].SectionEnglishName;
                var subjectArabicName = informations[3].SubjectArabicName;
                var subjectEnglishName = informations[3].SubjectEnglishName;

                // Homework message.
                var messageToPush = $"Mr / Mis {teacherEnglishName} Added new {type} for {schoolClassEnglishName} " +
                    $"{sectionEnglishName} for {subjectEnglishName} Subject";

                // Exam message.
                messageToPush += type.Equals("Exam") ? $" on {date}" : "";

                return messageToPush;
            }
            catch (Exception)
            {
                return "Fail Get data for notification!";
            }
        }

        private dynamic GetTeacherName(string staffID)
        {
            try
            {
                dynamic teacherInformation = null;

                string query = "SELECT StaffArabicName, StaffEnglishName FROM Staff " +
                               "WHERE StaffID = @StaffID; ";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StaffID", staffID);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                teacherInformation = new
                                {
                                    StaffArabicName = reader["StaffArabicName"].ToString(),
                                    StaffEnglishName = reader["StaffEnglishName"].ToString()
                                };
                            }
                        }
                    }
                }

                return teacherInformation;
            }
            catch (Exception ex)
            {
                return new Exception(ex.Message);
            }
        }

        private async Task SendToParents(IEnumerable<dynamic> guardians, string type, string notificationText, string title, DeviceType deviceType)
        {
            foreach (var guardian in guardians)
            {
                await _fireBaseService.SendNotificationAsync(guardian.guardiantoken, type, notificationText, title, deviceType);
            }
        }

        private async Task SendToParent(dynamic guardian, string type, string notificationText, string title, DeviceType deviceType)
        {
            await _fireBaseService.SendNotificationAsync(guardian.guardiantoken, type, notificationText, title, deviceType);
        }

        private int GetNotificaitonTypeId(string type)
        {
            switch (type)
            {
                case "Homework":
                    return (int)NotificationsTypes.Homework;
                case "Exam":
                    return (int)NotificationsTypes.Exam;
                case "Attendance":
                    return (int)NotificationsTypes.Attendance;
                default:
                    return -1;
            }
        }
    }
}