using UnityEngine;

namespace Services.AppInfo.Abstractions
{
    public abstract class AppInfoService
    {
        public string BundleId() => 
            Application.identifier;

        public string AppVersion() => 
            Application.version;

        public string UnityVersion() => 
            Application.unityVersion;

        public string SDKVersion() => 
            throw new System.NotImplementedException();

        public string GameId() => 
            throw new System.NotImplementedException();
    }
}