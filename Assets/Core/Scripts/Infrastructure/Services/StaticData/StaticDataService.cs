using System.Linq;
using System.Collections.Generic;
using Infrastructure.StaticData;
using Infrastructure.StaticData.ItemObjects;
using UnityEngine;
using UI.Window;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameConfigPath = "StaticData/Balance/GameConfig";
        private const string GameBalancePath = "StaticData/Balance/Balance";
        private const string InputConfigPath = "StaticData/Balance/InputConfig";
        private const string AudioStaticDataPath = "StaticData/Balance/AudioConfig";
        private const string FxEffectStaticDataPath = "StaticData/Balance/FxEffectStaticData";
        private const string PathResourcesConfigPath = "StaticData/Balance/PathConfig";
        private const string KeyWordsStaticDataPath = "StaticData/Balance/KeyWordConfig";
        private const string PlayerConfigPath = "StaticData/Entities/PlayerConfig";
        private const string WindowsStaticDataPath = "StaticData/WindowsStaticData";
        private const string ItemsObjectStaticDataPath = "StaticData/ItemsObject";

        private GameStaticData _gameStaticData;
        private BalanceStaticData _balanceStaticData;
        private AudioStaticData _audioStaticData;
        private InputStaticData _inputStaticData;
        private PlayerStaticData _playerStaticData;
        private PathResourcesStaticData _pathResourcesStaticData;
        private KeyWordsStaticData _keyWordsStaticData;

        private Dictionary<WindowTypeId, WindowConfig> _windowConfigs;
        private Dictionary<FxTypeId, FxEffectConfig> _effectConfigs;
        private Dictionary<string, ItemObjectData> _itemsObjectData;

        public BalanceStaticData Balance => _balanceStaticData;
        public GameStaticData GameConfig => _gameStaticData;
        public AudioStaticData AudioConfig => _audioStaticData;
        public PathResourcesStaticData PathResourcesConfig => _pathResourcesStaticData;
        public InputStaticData InputConfig => _inputStaticData;
        public PlayerStaticData PlayerConfig => _playerStaticData;
        public KeyWordsStaticData KeyWordsConfig => _keyWordsStaticData;
        public IEnumerable<ItemObjectData> AllItemsObject => _itemsObjectData.Values;


        public void LoadData()
        {
            _gameStaticData = Resources
                .Load<GameStaticData>(GameConfigPath);

            _audioStaticData = Resources
                .Load<AudioStaticData>(AudioStaticDataPath);

            _balanceStaticData = Resources
                .Load<BalanceStaticData>(GameBalancePath);

            _inputStaticData = Resources
                .Load<InputStaticData>(InputConfigPath);
            
            _playerStaticData = Resources
                .Load<PlayerStaticData>(PlayerConfigPath);

            _pathResourcesStaticData = Resources
                .Load<PathResourcesStaticData>(PathResourcesConfigPath);
            
            _keyWordsStaticData = Resources
                .Load<KeyWordsStaticData>(KeyWordsStaticDataPath);

            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowsStaticDataPath)
                .Configs.ToDictionary(x => x.WindowTypeId, x => x);
            
            _effectConfigs = Resources
                .Load<FxEffectStaticData>(FxEffectStaticDataPath)
                .FxEffectContainers.ToDictionary(x => x.FxType, x => x);
            
            _itemsObjectData = Resources
                .LoadAll<ItemObjectData>(ItemsObjectStaticDataPath)
                .ToDictionary(x => x.ItemID, x => x);
        }

        public WindowConfig ForWindow(WindowTypeId windowTypeId) => 
            _windowConfigs[windowTypeId];
        
        public FxEffectConfig ForFxEffect(FxTypeId fxTypeId) => 
            _effectConfigs[fxTypeId];
    }
}