using SmartSchoolLifeAPI.Core.Models;
using System.Collections.Generic;


namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface ITeacherClassScheduleRepository : IRepository<TeacherClassScheduleModel>
    {
        Dictionary<string, List<TeacherClassScheduleModel>> GetTeacherClassSchedule(int schoolID, string staffID, int timeTableType);
    }
}
