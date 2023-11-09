using Services.Analytics.Events;

namespace Services.Analytics
{
    public interface IAnalyticService
    {
        void Send(IAnalyticEvent analyticEvent);
    }
}