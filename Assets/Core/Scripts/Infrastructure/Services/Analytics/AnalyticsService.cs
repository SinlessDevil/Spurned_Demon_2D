using System;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Services.Analytics.Events;
using Infrastructure.Services.Analytics.Events.Behaviour;
using Infrastructure.Services.DeviceData.Abstractions;
using Infrastructure.Services.PersistenceProgress;
using Infrastructure.Services.PersistenceProgress.Analytic;
using UnityEngine;

namespace Infrastructure.Services.Analytics
{
    public class AnalyticService : IAnalyticService
    {
        public void Send(IAnalyticEvent analyticEvent)
        {
            string data = GetDataAsString(analyticEvent);
            
            Debug.Log($"<color=#ffff33ff>{analyticEvent.Name}</color> sending:\n{data}");
        }

        private string GetDataAsString(IAnalyticEvent analyticEvent)
        {
            string data = String.Empty;
            foreach (KeyValuePair<string, object> pair in analyticEvent.Data)
                data += pair.Key + " =>> " + pair.Value + "\n";
            
            return data;
        }
    }
    
    public class AnalyticEnrichService : IAnalyticService
    {
        private readonly IAnalyticService _analyticService;
        private readonly IDeviceDataService _deviceData;
        private readonly AnalyticsData _analyticsData;

        public AnalyticEnrichService(IAnalyticService analyticService, IPersistenceProgressService progressService, IDeviceDataService deviceData)
        {
            _analyticService = analyticService;
            _deviceData = deviceData;
            _analyticsData = progressService.AnalyticsData;
        }

        public void Send(IAnalyticEvent analyticEvent)
        {
            EnrichedEvent enrichedEvent = new EnrichedEvent(analyticEvent);
            
            enrichedEvent.AddData("session_id", _analyticsData.CurrentSession.Id);
            enrichedEvent.AddData("session_number", _analyticsData.SessionAmount);
            enrichedEvent.AddData("timezone_offset", GetTimezoneOffset());
            enrichedEvent.AddData("average_fps", _analyticsData.CurrentSession.FPS);
            enrichedEvent.AddData("connection_state", _deviceData.IsInternetEnabled());
            enrichedEvent.AddData("wifi_state", _deviceData.IsWifiEnabled());
            enrichedEvent.AddData("processor_frequency", _deviceData.ProcessorFrequency());
            enrichedEvent.AddData("processor_count", _deviceData.ProcessorCount());
            enrichedEvent.AddData("processor_type", _deviceData.ProcessorType());
            
            _analyticService.Send(enrichedEvent);
        }

        private string GetTimezoneOffset()
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            TimeSpan offset = localTimeZone.GetUtcOffset(DateTime.Now);
            return offset.TotalSeconds.ToString(CultureInfo.InvariantCulture);
        }
    }
}