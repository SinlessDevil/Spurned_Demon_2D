using System.Collections.Generic;

namespace Infrastructure.Services.Analytics.Events.Behaviour
{
    public class AppUpdate : IAnalyticEvent
    {
        public string Name => "app_update";
        public IReadOnlyDictionary<string, object> Data { get; }

        public AppUpdate(string version)
        {
            Data = new Dictionary<string, object>()
            {
                ["version"] = version
            };
        }
    }
}