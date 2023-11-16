using Infrastructure.Services.DeviceData.Abstractions;

namespace Infrastructure.Services.DeviceData
{
    class EditorDeviceDataService : DeviceDataService, IDeviceDataService
    {
        public string OperatingSystemVersion()
        {
            return "editor_"+ OperatingSystem();
        }

        public string Platform()
        {
            return "editor";
        }
    }
}