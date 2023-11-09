using Services.DeviceData.Abstractions;

namespace Services.DeviceData
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