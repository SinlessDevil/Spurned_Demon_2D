using Infrastructure.Services.PersistenceProgress.Analytic;
using Infrastructure.Services.PersistenceProgress.Player;

namespace Infrastructure.Services.PersistenceProgress
{
    public class PersistenceProgressService : IPersistenceProgressService
    {
        public PlayerData PlayerData { get; set; }
        public AnalyticsData AnalyticsData { get; set; }
    }
}