using SmartSchoolLifeAPI.Core.DTOs;
using SmartSchoolLifeAPI.Core.Models.Attendance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface IAttendanceRepository : IRepository<AttendanceModel>
    {
        AttendanceModel GetStudentAttendance(string studentID, string date);
        dynamic GetTeacherScheduleDays(int schoolId, string schoolYear,
            string teacherId, int sectionID, int timeTableType);
        dynamic GetTeacherScheduleSessionOrders(int schoolId, string schoolYear,
            string teacherId, int sectionID, int weekDay, int timeTableType);
        Task AddAttendanceByTeacher(List<AttendanceInsertDTO> model, int schoolId, string schoolYear, int sessionNumber,
            string date, string teacherId);
        dynamic GetAbsenceReasons();
        Task AddAttendanceByParent(StudentAttendanceByParentDTO absenceModel);
        bool GetStudentAbsenceOnBusStatus(int schoolId, string studentId, string busNumber, string tripDate, string tripTime, int direction);
        void SwitchStudentAttendanceOnBus(int schoolId, string studentId, string busNumber, string tripDate, string tripTime, int direction);
    }
}
