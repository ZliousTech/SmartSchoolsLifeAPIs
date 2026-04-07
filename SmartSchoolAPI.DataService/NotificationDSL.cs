using SmartSchool.Core.DataEntityTier;
using SmartSchool.FireBase.Service;
using SmartSchoolAPI.DataServiceFactory;
using SmartSchoolAPI.Entities;
using SmartSchoolAPI.IDataService;
using System;
using System.Threading.Tasks;

namespace SmartSchoolAPI.DataService
{
    public class NotificationDSL : INotificationDSL
    {
        private IFireBaseService _fireBaseService { get; } = new FireBaseService();

        public async Task<BaseResponseDTO<DeviceRegistrar_DTO>> GetDevRegIdByAttendantId(string ownerId)
        {
            var deviceRegistrar = await SmartSchoolAPIDataSevice_DeviceRegistrar.GetDeviceRegistrarByOwnerId(ownerId);
            if (deviceRegistrar == null)
            {
                return BaseResponseDSL<DeviceRegistrar_DTO>.CreateGenericResponse(true, null, $"No date found related to {nameof(ownerId)}: {ownerId}");
            }

            return BaseResponseDSL<DeviceRegistrar_DTO>.CreateGenericResponse(true, deviceRegistrar);
        }

        public async Task<BaseResponseDTO<DeviceRegistrar_DTO>> UpdateDeviceRegCode(UpdateDeviceRegistrarRequest updateDeviceRegistrarRequest)
        {
            var deviceRegistrar = await SmartSchoolAPIDataSevice_DeviceRegistrar.GetDeviceRegistrarByOwnerId(updateDeviceRegistrarRequest.OwnerId);
            if (deviceRegistrar == null)
            {
                return BaseResponseDSL<DeviceRegistrar_DTO>.CreateGenericResponse(true, null, "No Data Found");
            }

            deviceRegistrar.DeviceRegistrationCode = updateDeviceRegistrarRequest.DeviceRegistrationCode;
            deviceRegistrar.LastLoggedDeviceType = updateDeviceRegistrarRequest.DeviceType;

            var updatedDeviceRegistrar = await SmartSchoolAPIDataSevice_DeviceRegistrar.Upsert(deviceRegistrar);

            return BaseResponseDSL<DeviceRegistrar_DTO>.CreateGenericResponse(true, updatedDeviceRegistrar);
        }

        public async Task<BaseResponseDTO<DeviceRegistrar_DTO>> AddDeviceRegCode(AddDeviceRegistrarRequest addDeviceRegistrarRequest)
        {
            var registeredToken = await SmartSchoolAPIDataSevice_DeviceRegistrar.GetDeviceRegistrarByOwnerId(addDeviceRegistrarRequest.OwnerId);

            var requestedDeviceRegistrar = new DeviceRegistrar_DTO
            {
                Id = registeredToken?.Id ?? 0,
                DeviceRegistrationCode = addDeviceRegistrarRequest.DeviceRegistrationCode,
                IsDeviceRegistrationActive = -1,
                OwnerId = addDeviceRegistrarRequest.OwnerId,
                OwnerMobileNumber = addDeviceRegistrarRequest.OwnerMobileNumber,
                OwnerType = addDeviceRegistrarRequest.OwnerType.ToString(),
                RegistrationDate = DateTime.UtcNow,
                LastLoggedDeviceType = addDeviceRegistrarRequest.DeviceType
            };

            var deviceRegistrar = await SmartSchoolAPIDataSevice_DeviceRegistrar.Upsert(requestedDeviceRegistrar);

            return BaseResponseDSL<DeviceRegistrar_DTO>.CreateGenericResponse(true, deviceRegistrar);
        }

        public async Task<BaseResponseDTO<string>> SendFCM(string receiverToken, string title, string message, string type, string sound)
        {
            var lastLoggedDeviceType = (await SmartSchoolAPIDataSevice_DeviceRegistrar.GetDeviceRegistrarByReceiverToken(receiverToken))?.LastLoggedDeviceType;

            if (lastLoggedDeviceType == null)
            {
                return BaseResponseDSL<string>.CreateGenericResponse(false, string.Empty, $"Invalid {nameof(receiverToken)} data.");
            }

            await _fireBaseService.SendNotificationAsync(receiverToken, lastLoggedDeviceType.Value, type, message, title, sound);

            return BaseResponseDSL<string>.CreateGenericResponse(true, "Notification sent.");
        }
    }
}
