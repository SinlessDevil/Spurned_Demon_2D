using System.Text.RegularExpressions;
using Infrastructure.Services.DeviceData.Abstractions;

namespace Infrastructure.Services.DeviceData
{
    class IOSDeviceDataService : DeviceDataService, IDeviceDataService
    {
        public string Platform()
        {
            return "ios";
        }

        public string OperatingSystemVersion()
        {
            Regex regex = new Regex(@"[0-9.]+");
            if (regex.IsMatch(OperatingSystem()))
            {
                MatchCollection matches = regex.Matches(OperatingSystem());
                if (matches.Count > 0)
                    return matches[0].ToString();
            }
            else
            {
                string[] split = OperatingSystem().Split(' ');
                if (split.Length > 0)
                    return split[split.Length - 1];
            }
            
            return OperatingSystem();
        }
    }
}