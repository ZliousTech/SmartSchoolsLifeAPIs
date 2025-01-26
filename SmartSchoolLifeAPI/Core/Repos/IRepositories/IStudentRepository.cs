using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface IStudentRepository
    {
        IEnumerable<dynamic> GetStudentsData();
    }
}