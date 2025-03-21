using SmartSchool.FireBase.Service;
using SmartSchoolLifeAPI.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public interface IPushNotificationHandler
    {
        Task SendPushNotification(string staffId, int schoolClassId, int sectionId, int subjectId,
            string type, IEnumerable<dynamic> guardians, string date = "", DeviceType deviceType = DeviceType.Android);
        Task SendPushNotification(string staffId, string type, IEnumerable<AttendanceInsertDTO> students, DeviceType deviceType = DeviceType.Android);
        IEnumerable<dynamic> GetParentsSection(int sectionId);
    }
}
