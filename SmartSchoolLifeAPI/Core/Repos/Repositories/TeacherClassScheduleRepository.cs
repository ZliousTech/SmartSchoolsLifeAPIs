using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class TeacherClassScheduleRepository : ITeacherClassScheduleRepository
    {
        private readonly List<string> _daysList = new List<string>
            { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        public IEnumerable<TeacherClassScheduleModel> GetAll()
        {
            return new List<TeacherClassScheduleModel>();
        }

        public TeacherClassScheduleModel GetById(int id)
        {
            return new TeacherClassScheduleModel();
        }

        public TeacherClassScheduleModel Add(TeacherClassScheduleModel entity)
        {
            return new TeacherClassScheduleModel();
        }

        public void Update(TeacherClassScheduleModel entity)
        {

        }

        public void Delete(int id)
        {

        }

        private IEnumerable<TeacherClassScheduleModel> PrepareSchoolClassSchedule(int schoolID, string staffID, int timeTableType)
        {
            IEnumerable<dynamic> teacherClassScheduleList = new List<dynamic>();

            // -1 is Manual Time Table, and 0 is Automatic Time Table
            string _timeTableType = (timeTableType == -1) ? "ManualTimetable" : "AutomaticTimetable";
            string _academicYear = new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear;

            // In case you need them.
            SchoolSettings schoolSettings = new SchoolSettingsRepository().GetById(schoolID);

            string query = "SELECT t.TimetableItemID AS ID, t.TeacherID, " +
                "f.StaffArabicName, f.StaffEnglishName, " +
                "sc.SchoolClassArabicName, sc.SchoolClassEnglishName, " +
                "c.SectionCode, c.SectionArabicName, c.SectionEnglishName, " +
                "j.SubjectArabicName, j.SubjectEnglishName, " +
                "s.WeekDay, s.SessionDayOrder, t.ItemRGBColor " +
                "FROM " + _timeTableType + " TableType " +
                "INNER JOIN TimetableItems t ON TableType.TimetableItemID = t.TimetableItemID " +
                "INNER JOIN Staff f ON t.TeacherID = f.StaffID " +
                "INNER JOIN Sessions s ON TableType.SessionID = s.SessionID " +
                "INNER JOIN Subjects j ON t.SubjectID = j.SubjectID " +
                "INNER JOIN Sections c ON t.SectionID = c.SectionID " +
                "INNER JOIN SchoolClasses sc ON t.SchoolClassID = sc.SchoolClassID " +
                "WHERE TableType.SchoolID = @SchoolID AND TableType.SchoolYear = " + _academicYear + " " +
                "AND t.TeacherID = @TeacherID " +
                "ORDER BY s.WeekDay, s.SessionDayOrder ASC ";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolID);
                    comm.Parameters.AddWithValue("@TeacherID", staffID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        teacherClassScheduleList = reader.MapAll();
                    }
                }
            }

            return teacherClassScheduleList.Any() ?
                teacherClassScheduleList.MapListTo<TeacherClassScheduleModel>() : null;
        }

        public Dictionary<string, List<TeacherClassScheduleModel>> GetTeacherClassSchedule(int schoolID, string staffID, int timeTableType)
        {
            var groupedSchedule = PrepareSchoolClassSchedule(schoolID, staffID, timeTableType)?
                .GroupBy(s => s.WeekDay)
                .ToDictionary(
                    group => _daysList[group.Key],
                    group => group.ToList()
                );

            return groupedSchedule;
        }

    }
}