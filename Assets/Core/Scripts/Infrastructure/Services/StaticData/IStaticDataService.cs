using StaticData;
using Window;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        GameStaticData GameConfig { get; }
        AudioStaticData AudioConfig { get; }
        BalanceStaticData Balance { get; }

        void LoadData();

        WindowConfig ForWindow(WindowTypeId windowTypeId);
    }
}