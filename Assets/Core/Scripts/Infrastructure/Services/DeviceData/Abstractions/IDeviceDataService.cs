namespace Services.DeviceData.Abstractions
{
    public interface IDeviceDataService
    {
        string Platform();
        string OperatingSystemVersion();
        string DeviceId();
        string OperatingSystem();
        string DeviceModel();
        string DeviceLanguage();
        bool IsInternetEnabled();
        bool IsWifiEnabled();
        float ProcessorFrequency();
        int ProcessorCount();
        string ProcessorType();
    }
}