using System.Collections.Generic;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface IStaffRepository
    {
        IEnumerable<dynamic> GetStaffData();
    }
}
