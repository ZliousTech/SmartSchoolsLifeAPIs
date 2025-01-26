using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Models.Shared
{
    public interface IMapper<T> where T : class
    {
        List<T> MapAll(SqlDataReader reader);

        T MapSingle(SqlDataReader reader);
    }
}
