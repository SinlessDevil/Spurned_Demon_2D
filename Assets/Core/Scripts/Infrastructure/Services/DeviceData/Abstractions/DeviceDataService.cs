using UnityEngine;

namespace Services.DeviceData.Abstractions
{
    public abstract class DeviceDataService
    {
        public string DeviceId()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public string OperatingSystem()
        {
            return SystemInfo.operatingSystem;
        }

        public string DeviceModel()
        {
            return SystemInfo.deviceModel;
        }

        public string DeviceLanguage()
        {
            return Application.systemLanguage.ToString();
        }

        public bool IsInternetEnabled()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public bool IsWifiEnabled()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }

        public float ProcessorFrequency()
        {
            return SystemInfo.processorFrequency;
        }

        public int ProcessorCount()
        {
            return SystemInfo.processorCount;
        }

        public string ProcessorType()
        {
            return SystemInfo.processorType;
        }
    }
}