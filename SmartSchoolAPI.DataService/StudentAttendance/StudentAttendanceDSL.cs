using SmartSchool.Core.DataEntityTier;
using SmartSchool.Core.Enumerators;
using SmartSchoolAPI.DataServiceFactory;
using SmartSchoolAPI.Entities;
using SmartSchoolAPI.IDataService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchoolAPI.DataService
{
    public class StudentAttendanceDSL : IStudentAttendanceDSL
    {
        public async Task<BaseResponseDTO<List<StudentAttendancesResponse>>> GetStudentsAttendancePerTeacher(StudentAttendancesRequest studentAttendancesRequest)
        {
            var requestedStudentAttendance = ValidateAndMapStudnetAttendanceRequest(studentAttendancesRequest, out List<string> validationMessage);

            if (validationMessage.Any())
            {
                return BaseResponseDSL<List<StudentAttendancesResponse>>.CreateGenericResponse(false, new List<StudentAttendancesResponse>(), string.Join(", ", validationMessage));
            }

            var studentAttendance = await SmartSchoolAPIDataService_StudentAttendance.GetStudentsAttendancePerTeacher(requestedStudentAttendance.SchoolId,
                                                                                                                      requestedStudentAttendance.SchoolClassId,
                                                                                                                      requestedStudentAttendance.SectionId,
                                                                                                                      requestedStudentAttendance.TeacherId,
                                                                                                                      requestedStudentAttendance.AttendanceDate,
                                                                                                                      (AttendanceTypeEnum?)requestedStudentAttendance.AttendanceType);

            return BaseResponseDSL<List<StudentAttendancesResponse>>.CreateGenericResponse(true, MapToStudentAttendanceResponse(studentAttendance), string.Empty);
        }

        private StudentAttendancesRequest ValidateAndMapStudnetAttendanceRequest(StudentAttendancesRequest studentAttendancesRequest,
                                                                                 out List<string> validationMessage)
        {
            validationMessage = new List<string>();

            if (studentAttendancesRequest == null)
            {
                validationMessage.Add("Request body can't be empty");
                return null;
            }

            if (studentAttendancesRequest.SchoolId <= 0)
            {
                validationMessage.Add($"{nameof(studentAttendancesRequest.SchoolId)} must be more than 0");
            }

            if (string.IsNullOrWhiteSpace(studentAttendancesRequest.TeacherId))
            {
                validationMessage.Add($"{nameof(studentAttendancesRequest.TeacherId)} is required");
            }

            DateTime attendanceDate = DateTime.UtcNow.Date;
            if (string.IsNullOrWhiteSpace(studentAttendancesRequest.AttendanceDate) ||
                !DateTime.TryParseExact(studentAttendancesRequest.AttendanceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out attendanceDate))

            {
                validationMessage.Add($"{nameof(studentAttendancesRequest.AttendanceDate)} is required and must be in format [dd/MM/yyyy]");
            }

            if (studentAttendancesRequest.AttendanceType.HasValue && studentAttendancesRequest.AttendanceType != 0 &&
                !Enum.IsDefined(typeof(AttendanceTypeEnum), studentAttendancesRequest.AttendanceType.Value))
            {
                var validEnumValues = string.Join(", ", Enum.GetValues(typeof(AttendanceTypeEnum))
                                        .Cast<AttendanceTypeEnum>()
                                        .Select(e => $"{(int)e} ({e})"));

                validationMessage.Add($"{nameof(studentAttendancesRequest.AttendanceType)} must be one of: {validEnumValues}");
            }

            return new StudentAttendancesRequest
            {
                SchoolId = studentAttendancesRequest.SchoolId,
                SchoolClassId = (!studentAttendancesRequest.SchoolClassId.HasValue || studentAttendancesRequest.SchoolClassId == 0) ?
                                null : studentAttendancesRequest.SchoolClassId,
                SectionId = (!studentAttendancesRequest.SectionId.HasValue || studentAttendancesRequest.SectionId == 0) ?
                                null : studentAttendancesRequest.SectionId,
                TeacherId = studentAttendancesRequest.TeacherId,
                AttendanceDate = attendanceDate.ToString(),
                AttendanceType = (!studentAttendancesRequest.AttendanceType.HasValue || studentAttendancesRequest.AttendanceType == 0) ?
                                null : studentAttendancesRequest.AttendanceType
            };
        }

        private List<StudentAttendancesResponse> MapToStudentAttendanceResponse(List<StudentAttendances> studentAttendances)
        {
            if (studentAttendances == null || !studentAttendances.Any())
            {
                return new List<StudentAttendancesResponse>();
            }

            return studentAttendances.Select(s => new StudentAttendancesResponse
            {
                StudentId = s.StudentId,
                StudentArabicName = s.StudentName,
                StudentEnglishName = s.StudentEnglishName,
                StudentArabicClass = s.StudentClass,
                StudentEnglishClass = s.StudentEnglishClass,
                SectionId = s.SectionId,
                StudentArabicSection = s.StudentSection,
                StudentEnglishSection = s.StudentEnglishSection,
                StudentTotalAbsence = s.StudentTotalAbsence,
                StudenTotalPartialAttendace = s.StudenTotalPartialAttendace,
                IsAbsence = s.IsAbsence,
                IsManualAttendance = s.IsManualAttendance
            }).ToList();
        }
    }
}