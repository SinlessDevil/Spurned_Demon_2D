using System;
using Services.AppInfo.Abstractions;

namespace Services.AppInfo
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