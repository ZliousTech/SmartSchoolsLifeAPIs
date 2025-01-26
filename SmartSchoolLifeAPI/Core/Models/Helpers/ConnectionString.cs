using System.Configuration;

namespace SmartSchoolLifeAPI.Core.Models.Shared
{
    public sealed class ConnectionString
    {
        public static string ConnStr() => ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString;

    }
}