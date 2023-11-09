using Services.DeviceData.Abstractions;

namespace Services.DeviceData
{
    class AndroidDeviceDataService : DeviceDataService, IDeviceDataService
    {
        public string OperatingSystemVersion()
        {
            string[] split = OperatingSystem().Split(' ');
            if (split.Length > 0)
                return $"Android {split[split.Length - 1]}";
            
            return OperatingSystem();
        }

        public string Platform()
        {
            return "android";
        }
    }
}