using System.Collections.Generic;
using Infrastructure.StaticData;
using Infrastructure.StaticData.ItemObjects;
using UI.Window;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        GameStaticData GameConfig { get; }
        AudioStaticData AudioConfig { get; }
        BalanceStaticData Balance { get; }
        PathResourcesStaticData PathResourcesConfig { get; }
        
        InputStaticData InputConfig { get; }
        PlayerStaticData PlayerConfig { get; }
        KeyWordsStaticData KeyWordsConfig { get; }
        IEnumerable<ItemObjectData> AllItemsObject { get; }

        void LoadData();

        WindowConfig ForWindow(WindowTypeId windowTypeId);
        FxEffectConfig ForFxEffect(FxTypeId fxTypeId);
    }
}