using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services.Analytics.Events.Behaviour
{
    public class FirstOpen : IAnalyticEvent
    {
        public string Name => "first_open";
        public IReadOnlyDictionary<string, object> Data { get; }

        public FirstOpen(long firstOpenTimestamp)
        {
            Data = new Dictionary<string, object>()
            {
                ["first_open_time"] = firstOpenTimestamp
            };
        }
    }
    
    public class EnrichedEvent : IAnalyticEvent
    {
        private Dictionary<string, object> _data;

        public EnrichedEvent(IAnalyticEvent decoratingEvent)
        {
            Name = decoratingEvent.Name;
            _data = decoratingEvent.Data.ToDictionary(x => x.Key, x => x.Value);
            Data = _data;
        }

        public IReadOnlyDictionary<string, object> Data { get; }

        public string Name { get; }

        public void AddData(string key, object value)
        {
            _data.Add(key, value);
        }
    }
}