namespace Services.AppInfo.Abstractions
{
    public interface IAppInfoService
    {
        string BundleId();
        string BuildNumber();
        string AppVersion();
        string UnityVersion();
        string SDKVersion();
        string GameId();
    }
}