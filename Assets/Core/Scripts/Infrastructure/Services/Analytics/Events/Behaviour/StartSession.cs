using System.Collections.Generic;

namespace Services.Analytics.Events.Behaviour
{
    public class StartSession : IAnalyticEvent
    {
        public string Name => "start_session";
        public IReadOnlyDictionary<string, object> Data { get; }

        public StartSession(int sessionAmount, long sessionId)
        {
            Data = new Dictionary<string, object>()
            {
                ["sessionAmount"] = sessionAmount,
                ["sessionId"] = sessionId
            };
        }
    }
}