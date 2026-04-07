using SmartSchool.Core.DataEntityTier;
using SmartSchool.Core.DataService;
using SmartSchool.Core.Enumerators;
using SmartSchool.Core.IDataService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolAPI.DataServiceFactory
{
    public sealed class SmartSchoolAPIDataService_StudentAttendance
    {
        private static ISmartSchool_To_IntegrationStudentAttendanceDSL _smartSchool_To_IntegrationStudentAttendanceDSL { get; } =
                new SmartSchool_To_IntegrationStudentAttendanceDSL();

        public static async Task<List<StudentAttendances>> GetStudentsAttendancePerTeacher(int schoolId, int? schoolClassId, int? sectionId,
                                                                                           string teacherId, string attendanceDate, AttendanceTypeEnum? attendanceType)
        {
            return await _smartSchool_To_IntegrationStudentAttendanceDSL.GetStudentsAttendancePerTeacher(schoolId, schoolClassId, sectionId,
                                                                                                         teacherId, attendanceDate, attendanceType);
        }

        public static async Task InsertQuickAttendanceWithNotification(List<QuickAttendanceData> absenceStudentData, int schoolId, string teacherId)
        {
            await _smartSchool_To_IntegrationStudentAttendanceDSL.InsertQuickAttendanceWithNotification(absenceStudentData, schoolId, teacherId);
        }
    }
}