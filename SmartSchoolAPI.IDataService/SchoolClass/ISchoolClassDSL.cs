using SmartSchoolAPI.Entities;
using SmartSchoolAPI.Entities.Response.SchoolClass;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolAPI.IDataService
{
    public interface ISchoolClassDSL
    {
        Task<BaseResponseDTO<List<TeacherSchoolClassResponse>>> GetTeacherSchoolClasses(string teacherId);
    }
}