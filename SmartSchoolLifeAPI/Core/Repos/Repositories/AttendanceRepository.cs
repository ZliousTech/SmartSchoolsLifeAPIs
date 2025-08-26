using Common.Base;
using SmartSchoolLifeAPI.Core.DTOs;
using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Attendance;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class AttendanceRepository : IAttendanceRepository
    {
        SmartSchoolsEntities2 db = new SmartSchoolsEntities2();
        private readonly PushNotificationHandler _pushNotificationHandler;
        private readonly List<string> _weekdays = new List<string>()
        {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday"
        };
        private readonly Dictionary<int, string> _sessionProperties = new Dictionary<int, string>
        {
            { 1, "FirstSession" },
            { 2, "SecondSession" },
            { 3, "ThirdSession" },
            { 4, "FourthSession" },
            { 5, "FifthSession" },
            { 6, "SixthSession" },
            { 7, "SeventhSession" },
            { 8, "EighthSession" }
        };

        public AttendanceRepository()
        {
            _pushNotificationHandler = new PushNotificationHandler();
        }


        public IEnumerable<AttendanceModel> GetAll()
        {
            return new List<AttendanceModel>();
        }

        public AttendanceModel GetById(int id)
        {
            return new AttendanceModel();
        }

        public AttendanceModel Add(AttendanceModel entity)
        {
            return new AttendanceModel();
        }

        public void Update(AttendanceModel entity)
        {

        }

        public void Delete(int id)
        {

        }

        private List<string> GetStudentSchoolPublicHolyDays(string studentID)
        {
            List<string> holyDaysDatesList = new List<string>();
            int _schoolID = 0;
            string query = "";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();

                query = "SELECT SchoolID FROM StudentSchoolDetails " +
               "WHERE StudentID = @StudentID";
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", studentID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _schoolID = Convert.ToInt32(reader["SchoolID"].ToString());
                        }
                    }
                }

                query = "SELECT a.StartingDate FROM AcademicCalendar a " +
                "INNER JOIN AcademicCalendars s ON a.CalendarID = s.CalendarID " +
                "WHERE s.SchoolID = @SchoolID";
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", _schoolID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            holyDaysDatesList.Add(DateTime.Parse(reader["StartingDate"].ToString()).ToString("dd/MM/yyyy"));
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return holyDaysDatesList.ToList();
        }

        public AttendanceModel GetStudentAttendance(string studentID, string date)
        {
            AttendanceModel studentAttendance = null;

            // Check is date is public Holyday or Future date?
            if (date.IsWeekend() || date.IsFutureDate() || GetStudentSchoolPublicHolyDays(studentID).Contains(date))
                return studentAttendance;

            string query = "SELECT ss.NumberofSessionsPerDay, a.AttendanceItemID, a.StudentID, a.AttendanceDate, " +
                "t.AttedanceTypeArabicText, t.AttedanceTypeEnglishText, " +
                "r.AbsenceReasonArabicText, r.AbsenceReasonEnglishText, " +
                "a.Description, a.ParentInformed, a.ParentMessage, a.AttendanceNote, " +
                "a.FirstSession, a.SecondSession, a.ThirdSession, a.FourthSession, " +
                "a.FifthSession, a.SixthSession, a.SeventhSession, a.EighthSession " +
                "FROM Attendance a " +
                "INNER JOIN AttendanceType t ON a.AttendanceType = t.AttendanceTypeID " +
                "INNER JOIN AbsenceReasons r ON a.AbsenceReason = r.AbsenceReasonID " +
                "INNER JOIN StudentSchoolDetails s ON a.StudentID = s.StudentID " +
                "INNER JOIN SchoolSettings ss ON s.SchoolID = ss.SchoolID " +
                "WHERE a.StudentID = @StudentID AND a.AttendanceDate = @AttendanceDate";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", studentID);
                    comm.Parameters.AddWithValue("@AttendanceDate", date.ConvertStringToDate().ToString("dd/MM/yyyy"));
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        studentAttendance = new AttendanceModel();
                        if (reader.Read())
                        {
                            studentAttendance.AttendanceItemID = Convert.ToInt32(reader["AttendanceItemID"]);
                            studentAttendance.StudentID = reader["StudentID"].ToString();
                            studentAttendance.AttendanceDate = reader["AttendanceDate"].ToString().Split(' ')[0];
                            studentAttendance.AttendanceTypeArabicText = reader["AttedanceTypeArabicText"].ToString();
                            studentAttendance.AttendanceTypeEnglishText = reader["AttedanceTypeEnglishText"].ToString();
                            studentAttendance.AbsenceReasonArabicText = reader["AbsenceReasonArabicText"].ToString();
                            studentAttendance.AbsenceReasonEnglishText = reader["AbsenceReasonEnglishText"].ToString();
                            studentAttendance.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader["Description"].ToString();
                            studentAttendance.ParentInformed = Convert.ToBoolean(reader["ParentInformed"]);
                            studentAttendance.ParentMessage = reader.IsDBNull(reader.GetOrdinal("ParentMessage")) ? null : reader["ParentMessage"].ToString();
                            studentAttendance.AttendanceNote = reader.IsDBNull(reader.GetOrdinal("AttendanceNote")) ? null : reader["AttendanceNote"].ToString();
                            studentAttendance.FirstSession = reader.IsDBNull(reader.GetOrdinal("FirstSession")) ? true : Convert.ToBoolean(reader["FirstSession"]);
                            studentAttendance.SecondSession = reader.IsDBNull(reader.GetOrdinal("SecondSession")) ? true : Convert.ToBoolean(reader["SecondSession"]);
                            studentAttendance.ThirdSession = reader.IsDBNull(reader.GetOrdinal("ThirdSession")) ? true : Convert.ToBoolean(reader["ThirdSession"]);
                            studentAttendance.FourthSession = reader.IsDBNull(reader.GetOrdinal("FourthSession")) ? true : Convert.ToBoolean(reader["FourthSession"]);
                            studentAttendance.FifthSession = reader.IsDBNull(reader.GetOrdinal("FifthSession")) ? true : Convert.ToBoolean(reader["FifthSession"]);
                            studentAttendance.SixthSession = reader.IsDBNull(reader.GetOrdinal("SixthSession")) ? true : Convert.ToBoolean(reader["SixthSession"]);
                            studentAttendance.SeventhSession = reader.IsDBNull(reader.GetOrdinal("SeventhSession")) ? true : Convert.ToBoolean(reader["SeventhSession"]);
                            studentAttendance.EighthSession = reader.IsDBNull(reader.GetOrdinal("EighthSession")) ? true : Convert.ToBoolean(reader["EighthSession"]);
                            studentAttendance.NumberofSessionsPerDay = Convert.ToInt32(reader["NumberofSessionsPerDay"]);
                        }
                        else
                        {
                            studentAttendance.AttendanceItemID = 0;
                            studentAttendance.StudentID = studentID;
                            studentAttendance.AttendanceDate = date;
                            studentAttendance.AttendanceTypeArabicText = "حضور كامل";
                            studentAttendance.AttendanceTypeEnglishText = "Full Attendance";
                            studentAttendance.AbsenceReasonArabicText = null;
                            studentAttendance.AbsenceReasonEnglishText = null;
                            studentAttendance.FirstSession = true;
                            studentAttendance.SecondSession = true;
                            studentAttendance.ThirdSession = true;
                            studentAttendance.FourthSession = true;
                            studentAttendance.FifthSession = true;
                            studentAttendance.SixthSession = true;
                            studentAttendance.SeventhSession = true;
                            studentAttendance.EighthSession = true;
                            studentAttendance.NumberofSessionsPerDay = SchoolSettingsRepository.GetStudentSchoolSettings(studentID).NumberofSessionsPerDay;
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return studentAttendance;
        }

        public dynamic GetTeacherScheduleDays(int schoolId, string schoolYear,
            string teacherId, int sectionID, int timeTableType)
        {
            string query = string.Empty;
            List<dynamic> timeTableItems = new List<dynamic>();
            List<dynamic> timeTables = new List<dynamic>();
            List<dynamic> sessions = new List<dynamic>();
            schoolYear = string.IsNullOrEmpty(schoolYear) ?
                new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear : schoolYear;


            #region TimeTableItems
            query = "SELECT * FROM TimetableItems " +
                "WHERE TeacherID = @TeacherID AND SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SectionID", sectionID);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        timeTableItems = reader.MapAll();
                    }
                }
            }
            var timeTableItemsObj = timeTableItems.MapListTo<TimetableItems>();
            #endregion

            #region TimeTables
            string timeTableTypeName = timeTableType == -1 ? "ManualTimetable" : "AutomaticTimetable";
            query = "SELECT * FROM " + timeTableTypeName +
                " WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear AND " +
                "TeacherID = @TeacherID AND SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    comm.Parameters.AddWithValue("@SchoolYear", schoolYear);
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SectionID", sectionID);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        timeTables = reader.MapAll();
                    }
                }
            }
            /* the ManualTimetable and AutomaticTimetable have the same attributes you can use any of them 
             for represent both of them */
            var timeTablesObj = timeTables.MapListTo<ManualTimetable>();
            #endregion

            #region Sessions
            query = "SELECT * FROM [Sessions] " +
                "WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear AND SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    comm.Parameters.AddWithValue("@SchoolYear", schoolYear);
                    comm.Parameters.AddWithValue("@SectionID", sectionID);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        sessions = reader.MapAll();
                    }
                }
            }
            var sessionsObj = sessions.MapListTo<Sessions>();
            #endregion

            var teachertableinfo = (from a in timeTableItemsObj
                                    join b in timeTablesObj
                                    on a.TeacherID equals b.TeacherID
                                    join c in sessionsObj
                                    on b.SessionID equals c.SessionID
                                    where a.TeacherID == teacherId
                                    && a.SectionID == sectionID
                                    select new
                                    {
                                        ID = c.WeekDay,
                                        Day = _weekdays[(int)c.WeekDay]
                                    }).GroupBy(t => t.ID)
                                      .Select(g => g.First())
                                      .OrderBy(t => t.ID)
                                      .ToList();

            return teachertableinfo.Any() ? teachertableinfo : null;
        }

        public dynamic GetTeacherScheduleSessionOrders(int schoolId, string schoolYear,
            string teacherId, int sectionID, int weekDay, int timeTableType)
        {
            string query = string.Empty;
            List<dynamic> timeTableItems = new List<dynamic>();
            List<dynamic> timeTables = new List<dynamic>();
            List<dynamic> sessions = new List<dynamic>();
            schoolYear = string.IsNullOrEmpty(schoolYear) ?
                new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear : schoolYear;


            #region TimeTableItems
            query = "SELECT * FROM TimetableItems " +
                "WHERE TeacherID = @TeacherID AND SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SectionID", sectionID);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        timeTableItems = reader.MapAll();
                    }
                }
            }
            var timeTableItemsObj = timeTableItems.MapListTo<TimetableItems>();
            #endregion

            #region TimeTables
            string timeTableTypeName = timeTableType == -1 ? "ManualTimetable" : "AutomaticTimetable";
            query = "SELECT * FROM " + timeTableTypeName +
                " WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear AND " +
                "TeacherID = @TeacherID AND SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    comm.Parameters.AddWithValue("@SchoolYear", schoolYear);
                    comm.Parameters.AddWithValue("@TeacherID", teacherId);
                    comm.Parameters.AddWithValue("@SectionID", sectionID);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        timeTables = reader.MapAll();
                    }
                }
            }
            /* the ManualTimetable and AutomaticTimetable have the same attributes you can use any of them 
             for represent both of them */
            var timeTablesObj = timeTables.MapListTo<ManualTimetable>();
            #endregion

            #region Sessions
            query = "SELECT * FROM [Sessions] " +
                "WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear AND SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    comm.Parameters.AddWithValue("@SchoolYear", schoolYear);
                    comm.Parameters.AddWithValue("@SectionID", sectionID);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        sessions = reader.MapAll();
                    }
                }
            }
            var sessionsObj = sessions.MapListTo<Sessions>();
            #endregion

            var teachertableinfo = (from a in timeTableItemsObj
                                    join b in timeTablesObj
                                    on a.TeacherID equals b.TeacherID
                                    join c in sessionsObj
                                    on b.SessionID equals c.SessionID
                                    where a.TeacherID == teacherId
                                    && a.SectionID == sectionID
                                    && c.WeekDay == weekDay
                                    select new
                                    {
                                        DayOrder = c.SessionDayOrder
                                    }).GroupBy(t => t.DayOrder)
                                      .Select(g => g.First())
                                      .OrderBy(t => t.DayOrder)
                                      .ToList();

            return teachertableinfo.Any() ? teachertableinfo : null;
        }

        public async Task AddAttendanceByTeacher(List<AttendanceInsertDTO> model,
            int schoolId, string schoolYear, int sessionNumber, string date, string teacherId)
        {
            if (sessionNumber < 1 || sessionNumber > 8)
                throw new Exception("Session Number must be between 1 and 8");

            if (model.Any())
            {
                schoolYear = string.IsNullOrEmpty(schoolYear) ?
                    new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear : schoolYear;

                foreach (var item in model)
                {
                    var studentAttendance = db.Attendances.Where(a => a.StudentID == item.StudentId
                        && a.AttendanceDate == date).FirstOrDefault();

                    if (studentAttendance != null)
                    {
                        studentAttendance.SchoolID = schoolId;
                        studentAttendance.SchoolYear = schoolYear;
                        studentAttendance.StudentID = item.StudentId;
                        studentAttendance.AttendanceDate = date;
                        studentAttendance.AbsenceReason = 8; // 8  for Other in Absence Reasons lookup table.
                        studentAttendance.ParentInformed = item.ParentInformed;
                        if (_sessionProperties.ContainsKey(sessionNumber))
                        {
                            var propertyName = _sessionProperties[sessionNumber];
                            var property = typeof(SmartSchoolLifeAPI.Attendance).GetProperty(propertyName);
                            property?.SetValue(studentAttendance, 0);
                        }
                        if (studentAttendance.FirstSession == 1 && studentAttendance.SecondSession == 1 &&
                            studentAttendance.ThirdSession == 1 && studentAttendance.FourthSession == 1 &&
                            studentAttendance.FifthSession == 1 && studentAttendance.SixthSession == 1 &&
                            studentAttendance.SeventhSession == 1 && studentAttendance.EighthSession == 1)
                        {
                            studentAttendance.AttendanceType = 3; // 3 for Absent.
                        }
                        else
                            studentAttendance.AttendanceType = 2; // 2 for Partial Attendace.

                        db.Attendances.AddOrUpdate(studentAttendance);
                        db.SaveChanges();
                    }
                    else
                    {
                        SmartSchoolLifeAPI.Attendance obj = new SmartSchoolLifeAPI.Attendance();
                        obj.SchoolID = schoolId;
                        obj.SchoolYear = schoolYear;
                        obj.StudentID = item.StudentId;
                        obj.AttendanceDate = date;
                        obj.AbsenceReason = 8; // 8  for Other.
                        obj.ParentInformed = item.ParentInformed;
                        if (_sessionProperties.ContainsKey(sessionNumber))
                        {
                            var propertyName = _sessionProperties[sessionNumber];
                            var property = typeof(SmartSchoolLifeAPI.Attendance).GetProperty(propertyName);
                            property?.SetValue(obj, 0);
                        }
                        if (obj.FirstSession == 1 && obj.SecondSession == 1 && obj.ThirdSession == 1 &&
                            obj.FourthSession == 1 && obj.FifthSession == 1 && obj.SixthSession == 1 &&
                            obj.SeventhSession == 1 && obj.EighthSession == 1)
                        {
                            obj.AttendanceType = 3; // 3 for Absent.
                        }
                        else
                            obj.AttendanceType = 2; // 2 for Partial Attendace.

                        db.Attendances.Add(obj);
                        db.SaveChanges();
                    }
                }

                await _pushNotificationHandler.SendPushNotification(teacherId, "Attendance", model);
            }
        }

        public async Task AddAttendanceByParent(StudentAttendanceByParentDTO absenceModel)
        {
            if (string.IsNullOrEmpty(absenceModel.ToDate))
                absenceModel.ToDate = absenceModel.FromDate;

            DateTime fromDate = DateTime.ParseExact(absenceModel.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(absenceModel.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (fromDate > toDate)
                throw new Exception("the fromDate must be less than or befor toDate");


            absenceModel.SchoolYear = string.IsNullOrEmpty(absenceModel.SchoolYear) ? new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear : absenceModel.SchoolYear;

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                string dateStr = date.ToString("dd/MM/yyyy");
                var studentAttendance = db.Attendances.Where(a => a.StudentID == absenceModel.StudentId && a.AttendanceDate == dateStr).FirstOrDefault();

                if (studentAttendance != null)
                {
                    studentAttendance.SchoolID = absenceModel.SchoolId;
                    studentAttendance.SchoolYear = absenceModel.SchoolYear;
                    studentAttendance.StudentID = absenceModel.StudentId;
                    studentAttendance.AttendanceDate = dateStr;
                    studentAttendance.AbsenceReason = 8; // Means "other" in AbsenceReasons Table 😇. 
                    studentAttendance.ParentInformed = 1;
                    studentAttendance.Description = string.Empty;
                    studentAttendance.AttendanceNote = string.IsNullOrEmpty(absenceModel.Note) ? string.Empty : absenceModel.Note;
                    foreach (var session in absenceModel.Sessions)
                    {
                        if (_sessionProperties.ContainsKey(session))
                        {
                            var propertyName = _sessionProperties[session];
                            var property = typeof(SmartSchoolLifeAPI.Attendance).GetProperty(propertyName);
                            property?.SetValue(studentAttendance, 0);
                        }
                    }
                    if (studentAttendance.FirstSession == 1 && studentAttendance.SecondSession == 1 &&
                        studentAttendance.ThirdSession == 1 && studentAttendance.FourthSession == 1 &&
                        studentAttendance.FifthSession == 1 && studentAttendance.SixthSession == 1 &&
                        studentAttendance.SeventhSession == 1 && studentAttendance.EighthSession == 1)
                    {
                        studentAttendance.AttendanceType = 3; // 3 for Absent.
                    }
                    else
                        studentAttendance.AttendanceType = 2; // 2 for Partial Attendace.

                    db.Attendances.AddOrUpdate(studentAttendance);
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(absenceModel.Attachment))
                    {
                        if (string.IsNullOrEmpty(absenceModel.Ext))
                            throw new Exception("When you upload an attachment the Ext must have a value!");
                        else
                        {
                            if (!absenceModel.Ext.StartsWith("."))
                                throw new Exception("the Ext value must start with dot(.) ");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(absenceModel.Ext))
                            throw new Exception("You must select a file, then add its extention!");
                    }

                    string query = "UPDATE Attendance " +
                                "SET Attachment = @Attachment, Ext = @Ext WHERE AttendanceItemID = @AttendanceItemID";

                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@Attachment", absenceModel.Attachment);
                            comm.Parameters.AddWithValue("@Ext", absenceModel.Ext);
                            comm.Parameters.AddWithValue("@AttendanceItemID", studentAttendance.AttendanceItemID);
                            comm.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    Attendance obj = new Attendance();
                    obj.SchoolID = absenceModel.SchoolId;
                    obj.SchoolYear = absenceModel.SchoolYear;
                    obj.StudentID = absenceModel.StudentId;
                    obj.AttendanceDate = dateStr;
                    obj.AbsenceReason = 8; // Means "other" in AbsenceReasons Table 😇. 
                    obj.ParentInformed = 1;
                    obj.Description = string.Empty;
                    obj.AttendanceNote = string.IsNullOrEmpty(absenceModel.Note) ? string.Empty : absenceModel.Note;
                    foreach (var session in absenceModel.Sessions)
                    {
                        if (_sessionProperties.ContainsKey(session))
                        {
                            var propertyName = _sessionProperties[session];
                            var property = typeof(SmartSchoolLifeAPI.Attendance).GetProperty(propertyName);
                            property?.SetValue(obj, 0);
                        }
                    }
                    if (obj.FirstSession == 1 && obj.SecondSession == 1 && obj.ThirdSession == 1 &&
                        obj.FourthSession == 1 && obj.FifthSession == 1 && obj.SixthSession == 1 &&
                        obj.SeventhSession == 1 && obj.EighthSession == 1)
                    {
                        obj.AttendanceType = 3; // 3 for Absent.
                    }
                    else
                        obj.AttendanceType = 2; // 2 for Partial Attendace.

                    db.Attendances.Add(obj);
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(absenceModel.Attachment))
                    {
                        if (string.IsNullOrEmpty(absenceModel.Ext))
                            throw new Exception("When you upload an attachment the Ext must have a value!");
                        else
                        {
                            if (!absenceModel.Ext.StartsWith("."))
                                throw new Exception("the Ext value must start with dot(.) ");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(absenceModel.Ext))
                            throw new Exception("You must select a file, then add its extention!");
                    }

                    string query = "UPDATE Attendance " +
                                "SET Attachment = @Attachment, Ext = @Ext WHERE AttendanceItemID = @AttendanceItemID";

                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@Attachment", absenceModel.Attachment);
                            comm.Parameters.AddWithValue("@Ext", absenceModel.Ext);
                            comm.Parameters.AddWithValue("@AttendanceItemID", obj.AttendanceItemID);
                            comm.ExecuteNonQuery();
                        }
                    }
                }

                await _pushNotificationHandler.SendPushNotification(absenceModel.StudentId, absenceModel.Sessions, _sessionProperties, "Attendance", DateTime.ParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
            }
        }

        public dynamic GetAbsenceReasons()
        {
            return db.AbsenceReasons;
        }

        public bool GetStudentAbsenceOnBusStatus(int schoolId, string studentId, string busNumber, string tripDate, string tripTime, int direction)
        {
            tripDate = SysBase.GetLastScheduledTripDate(schoolId);
            string query = "SELECT IsAbsenceByParent FROM ScheduledBusTrips " +
                "WHERE SchoolID = @SchoolID AND PassengerID = @StudentID AND BusNumber = @BusNumber AND TripDate = @TripDate AND TripTime = @TripTime AND Direction = @Direction";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    comm.Parameters.AddWithValue("@StudentID", studentId);
                    comm.Parameters.AddWithValue("@BusNumber", busNumber);
                    comm.Parameters.AddWithValue("@TripDate", tripDate);
                    comm.Parameters.AddWithValue("@TripTime", tripTime);
                    comm.Parameters.AddWithValue("@Direction", direction);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                            return Convert.ToBoolean(reader["IsAbsenceByParent"].ToString());
                        throw new ObjectNotFoundException("The current data does not have match recordes in database.");
                    }
                }
            }
        }

        public void SwitchStudentAttendanceOnBus(int schoolId, string studentId, string busNumber, string tripDate, string tripTime, int direction)
        {
            int noOfRecords;

            string query = "UPDATE ScheduledBusTrips " +
                "SET IsAbsenceByParent = CASE " +
                "WHEN IsAbsenceByParent = 1 THEN 0 " +
                "ELSE 1 END " +
                "WHERE SchoolID = @SchoolID AND PassengerID = @StudentID AND BusNumber = @BusNumber AND TripDate = @TripDate AND TripTime = @TripTime AND Direction = @Direction";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    comm.Parameters.AddWithValue("@StudentID", studentId);
                    comm.Parameters.AddWithValue("@BusNumber", busNumber);
                    comm.Parameters.AddWithValue("@TripDate", tripDate);
                    comm.Parameters.AddWithValue("@TripTime", tripTime);
                    comm.Parameters.AddWithValue("@Direction", direction);

                    noOfRecords = comm.ExecuteNonQuery();
                }

                if (noOfRecords == 0)
                    throw new Exception("The current data doesn not have match recordes in database.");
            }
        }

        public string GetLastScheduledTripDate(int SchoolID)
        {
            string LSTripDate = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable ScheduledTripInfo =
                SysBase.GetDataTble("SELECT TOP 1 * " +
                            "FROM ScheduledBusTrips " +
                            "WHERE SchoolID = '" + SchoolID + "' " +
                            "ORDER BY TripPassengerID DESC");


            if (ScheduledTripInfo != null)
            {
                if (ScheduledTripInfo.Rows.Count > 0)
                {
                    LSTripDate = ScheduledTripInfo.Rows[0]["TripDate"].ToString();
                }
            }

            return LSTripDate;
        }
    }
}