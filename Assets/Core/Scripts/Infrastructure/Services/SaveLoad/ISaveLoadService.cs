using Infrastructure.Services.PersistenceProgress.Player;

namespace Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        PlayerData LoadProgress();
        void SaveProgress();
        void ResetProgress();
    }
}