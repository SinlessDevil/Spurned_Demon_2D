using System.Collections.Generic;

namespace Services.Analytics.Events
{
    public interface IAnalyticEvent
    {
        string Name { get; }
        IReadOnlyDictionary<string, object> Data { get; }
    }
}