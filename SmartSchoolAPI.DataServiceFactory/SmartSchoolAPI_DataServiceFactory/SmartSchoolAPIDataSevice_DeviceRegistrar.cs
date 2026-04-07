using SmartSchool.Core.DataEntityTier;
using SmartSchool.Core.DataService;
using SmartSchool.Core.IDataService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolAPI.DataServiceFactory
{
    public sealed class SmartSchoolAPIDataSevice_DeviceRegistrar
    {
        private static ISmartSchool_To_IntegrationDeviceRegistrar _smartSchool_To_IntegrationDeviceRegistrarDSL { get; } =
                new SmartSchool_To_IntegrationDeviceRegistrar();

        public static async Task<DeviceRegistrar_DTO> GetDeviceRegistrarByOwnerId(string ownerId)
        {
            return await _smartSchool_To_IntegrationDeviceRegistrarDSL.GetDeviceRegistrarByOwnerId(ownerId);
        }

        public static async Task<IEnumerable<DeviceRegistrar_DTO>> GetDeviceRegistrarByOwnerIds(List<string> ownerIds)
        {
            return await _smartSchool_To_IntegrationDeviceRegistrarDSL.GetDeviceRegistrarByOwnerIds(ownerIds);
        }

        public static async Task<DeviceRegistrar_DTO> GetDeviceRegistrarByReceiverToken(string receiverToken)
        {
            return await _smartSchool_To_IntegrationDeviceRegistrarDSL.GetDeviceRegistrarByReceiverToken(receiverToken);
        }

        public static async Task<DeviceRegistrar_DTO> Upsert(DeviceRegistrar_DTO deviceRegistrar)
        {
            return await _smartSchool_To_IntegrationDeviceRegistrarDSL.Upsert(deviceRegistrar);
        }
    }
}
