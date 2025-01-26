using SmartSchoolLifeAPI.Core.Models;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface ISystemSettingsperSchoolRepository : IRepository<SystemSettingsperSchool>
    {
        dynamic GetTimeTableType(int schoolID);
    }
}
