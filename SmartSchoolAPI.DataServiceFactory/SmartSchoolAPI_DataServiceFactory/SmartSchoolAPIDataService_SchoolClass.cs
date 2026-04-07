using SmartSchool.Core.DataEntityTier;
using SmartSchool.Core.DataService;
using SmartSchool.Core.IDataService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolAPI.DataServiceFactory
{
    public sealed class SmartSchoolAPIDataService_SchoolClass
    {
        private static ISmartSchool_To_IntegrationSchoolClassDSL _smartSchool_To_IntegrationSchoolClassDSL { get; } =
                new SmartSchool_To_IntegrationSchoolClassDSL();

        public static async Task<List<LiteSchoolClass>> GetTeacherSchoolClasses(string teacherId)
        {
            return await _smartSchool_To_IntegrationSchoolClassDSL.GetTeacherSchoolClasses(teacherId);
        }
    }
}