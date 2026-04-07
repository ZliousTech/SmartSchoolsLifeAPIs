using SmartSchool.Core.DataEntityTier;
using SmartSchoolAPI.Entities;
using System.Threading.Tasks;

namespace SmartSchoolAPI.IDataService
{
    public interface INotificationDSL
    {
        Task<BaseResponseDTO<DeviceRegistrar_DTO>> GetDevRegIdByAttendantId(string ownerId);
        Task<BaseResponseDTO<DeviceRegistrar_DTO>> UpdateDeviceRegCode(UpdateDeviceRegistrarRequest updateDeviceRegistrarRequest);
        Task<BaseResponseDTO<DeviceRegistrar_DTO>> AddDeviceRegCode(AddDeviceRegistrarRequest addDeviceRegistrarRequest);
        Task<BaseResponseDTO<string>> SendFCM(string receiverToken, string title, string message, string type, string sound);
    }
}
