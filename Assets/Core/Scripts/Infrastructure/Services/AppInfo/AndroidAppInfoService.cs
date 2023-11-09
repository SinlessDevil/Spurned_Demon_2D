using System;
using Services.AppInfo.Abstractions;

namespace Services.AppInfo
{
    class AndroidAppInfoService : AppInfoService, IAppInfoService
    {
        public string BuildNumber()
        {
            throw new NotImplementedException();
            //AndroidJavaClass compat = new AndroidJavaClass("androidx.core.content.pm.PackageInfoCompat");
            //AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            // var ca = up.Get<AndroidJavaObject>("currentActivity");
            //AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
            // var pInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", Application.identifier, 0);
            // return compat.Call<long>("getLongVersionCode", pInfo).ToString();
        }
    }
}