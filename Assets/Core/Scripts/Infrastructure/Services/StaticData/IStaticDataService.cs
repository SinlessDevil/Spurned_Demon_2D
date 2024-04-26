using Infrastructure.StaticData;
using UI.Window;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        GameStaticData GameConfig { get; }
        AudioStaticData AudioConfig { get; }
        BalanceStaticData Balance { get; }
        PathResourcesStaticData PathResourcesConfig { get; }
        
        PlayerStaticData PlayerConfig { get; }
        
        void LoadData();

        WindowConfig ForWindow(WindowTypeId windowTypeId);
        FxEffectConfig ForFxEffect(FxTypeId fxTypeId);
    }
}