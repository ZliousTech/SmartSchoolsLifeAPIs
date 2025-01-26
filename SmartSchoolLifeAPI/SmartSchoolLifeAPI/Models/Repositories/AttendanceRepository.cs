using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SmartSchoolLifeAPI.Models.Shared;
using SmartSchoolLifeAPI.Models.Attendance;
using SmartSchoolLifeAPI.Models.Extensions;
using System.Web.Mvc;

namespace SmartSchoolLifeAPI.Models.Repositories
{
    public class AttendanceRepository : IRepository<AttendanceModel>
    {
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
                    comm.Parameters.AddWithValue("@AttendanceDate", date); // Format dd/MM/yyyy.
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
                            studentAttendance.NumberofSessionsPerDay = new SchoolSettingsRepository().GetStudentSchoolSettings(studentID).NumberofSessionsPerDay;
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return studentAttendance;
        }
    }
}