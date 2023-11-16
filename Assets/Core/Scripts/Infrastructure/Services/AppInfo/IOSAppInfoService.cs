using System;
using Infrastructure.Services.AppInfo.Abstractions;

namespace Infrastructure.Services.AppInfo
{
    class IOSAppInfoService : AppInfoService, IAppInfoService
    {
        public string BuildNumber()
        {
            throw new NotImplementedException();
            //return iOSExterns.GetBuildNumber();
        }
    }
}