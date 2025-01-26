using SmartSchoolLifeAPI.Core.Models;
using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface ISchoolClassScheduleRepository : IRepository<SchoolClassScheduleModel>
    {
        Dictionary<string, List<SchoolClassScheduleModel>> GetSchoolClassSchedule(int schoolID, int schoolClassID, int sectionID, int timeTableType);
    }
}
