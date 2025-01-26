using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SmartSchoolLifeAPI.Models.Shared
{
    public sealed class ConnectionString
    {
        public static string ConnStr() => ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString;

    }
}