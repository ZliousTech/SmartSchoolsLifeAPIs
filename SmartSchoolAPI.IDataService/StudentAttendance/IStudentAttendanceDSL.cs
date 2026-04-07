using SmartSchoolAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolAPI.IDataService
{
    public interface IStudentAttendanceDSL
    {
        Task<BaseResponseDTO<List<StudentAttendancesResponse>>> GetStudentsAttendancePerTeacher(StudentAttendancesRequest studentAttendancesRequest);

        Task<BaseResponseDTO<string>> InsertQuickAttendanceWithNotification(QuickAttendanceRequest quickAttendanceRequest);
    }
}