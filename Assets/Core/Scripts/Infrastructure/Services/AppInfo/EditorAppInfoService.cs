using Infrastructure.Services.AppInfo.Abstractions;

namespace Infrastructure.Services.AppInfo
{
    class EditorAppInfoService : AppInfoService, IAppInfoService
    {
        public string BuildNumber()
        {
            return string.Empty;
        }
    }
}