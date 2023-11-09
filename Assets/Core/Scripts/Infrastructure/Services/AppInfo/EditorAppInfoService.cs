using Services.AppInfo.Abstractions;

namespace Services.AppInfo
{
    class EditorAppInfoService : AppInfoService, IAppInfoService
    {
        public string BuildNumber()
        {
            return string.Empty;
        }
    }
}