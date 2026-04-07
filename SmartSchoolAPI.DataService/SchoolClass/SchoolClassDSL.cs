using SmartSchool.Core.DataEntityTier;
using SmartSchoolAPI.DataServiceFactory;
using SmartSchoolAPI.Entities;
using SmartSchoolAPI.Entities.Response.SchoolClass;
using SmartSchoolAPI.IDataService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchoolAPI.DataService
{
    public class SchoolClassDSL : ISchoolClassDSL
    {
        public async Task<BaseResponseDTO<List<TeacherSchoolClassResponse>>> GetTeacherSchoolClasses(string teacherId)
        {
            ValidateTeacherSchoolClassRequest(teacherId, out List<string> validationMessage);

            if (validationMessage.Any())
            {
                return BaseResponseDSL<List<TeacherSchoolClassResponse>>.CreateGenericResponse(false, new List<TeacherSchoolClassResponse>(), string.Join(", ", validationMessage));
            }

            var teacherSchoolClasses = await SmartSchoolAPIDataService_SchoolClass.GetTeacherSchoolClasses(teacherId);

            return BaseResponseDSL<List<TeacherSchoolClassResponse>>.CreateGenericResponse(true, MapToStudentAttendanceResponse(teacherSchoolClasses));
        }

        private void ValidateTeacherSchoolClassRequest(string teacherId, out List<string> validationMessage)
        {
            validationMessage = new List<string>();

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                validationMessage.Add($"{nameof(teacherId)} is required.");
            }
        }

        private List<TeacherSchoolClassResponse> MapToStudentAttendanceResponse(List<LiteSchoolClass> teacherSchoolClasses)
        {
            if (teacherSchoolClasses == null || !teacherSchoolClasses.Any())
            {
                return new List<TeacherSchoolClassResponse>();
            }

            return teacherSchoolClasses.Select(c => new TeacherSchoolClassResponse
            {
                Id = c.Id,
                EnglishName = c.SchoolClassEnglishName,
                ArabicName = c.SchoolClassArabicName
            }).ToList();
        }
    }
}