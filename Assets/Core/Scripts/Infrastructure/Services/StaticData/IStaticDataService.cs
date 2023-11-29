using Infrastructure.StaticData;
using Window;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        GameStaticData GameConfig { get; }
        AudioStaticData AudioConfig { get; }
        PlayerStaticData PlayerConfig { get; }

        BalanceStaticData Balance { get; }

        void LoadData();

        WindowConfig ForWindow(WindowTypeId windowTypeId);
    }
}