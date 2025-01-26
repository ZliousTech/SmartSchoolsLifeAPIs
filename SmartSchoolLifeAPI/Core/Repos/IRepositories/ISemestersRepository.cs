using SmartSchoolLifeAPI.Core.Models.Settings.Semester;
using SmartSchoolLifeAPI.ViewModels;
using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface ISemestersRepository : IRepository<SemesterModel>
    {
        IEnumerable<SemesterVM> GetSchoolSemesters(int schoolId, string schoolYear);
    }
}