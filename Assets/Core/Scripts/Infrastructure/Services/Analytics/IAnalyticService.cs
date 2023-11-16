using Infrastructure.Services.Analytics.Events;

namespace Infrastructure.Services.Analytics
{
    public interface IAnalyticService
    {
        void Send(IAnalyticEvent analyticEvent);
    }
}