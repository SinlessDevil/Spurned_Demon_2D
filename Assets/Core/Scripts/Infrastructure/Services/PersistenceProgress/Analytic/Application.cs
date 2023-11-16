using System;

namespace Infrastructure.Services.PersistenceProgress.Analytic
{
    [Serializable]
    public class Application
    {
        public string Version;
        public string UnityVersion;
        public string BundleID;
    }
}