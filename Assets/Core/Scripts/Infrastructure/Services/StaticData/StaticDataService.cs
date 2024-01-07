using System.Linq;
using System.Collections.Generic;
using Infrastructure.StaticData;
using UnityEngine;
using Window;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameConfigPath = "StaticData/Balance/GameConfig";
        private const string GameBalancePath = "StaticData/Balance/Balance";
        private const string PlayerConfigPath = "StaticData/Balance/PlayerConfig";
        private const string WindowsStaticDataPath = "StaticData/WindowsStaticData";
        private const string FxEffectStaticDataPath = "StaticData/FxEffectStaticData";
        private const string AudioStaticDataPath = "StaticData/AudioConfig";

        private GameStaticData _gameStaticData;
        private BalanceStaticData _balanceStaticData;
        private AudioStaticData _audioStaticData;
        private PlayerStaticData _playerStaticData;

        private Dictionary<WindowTypeId, WindowConfig> _windowConfigs;
        private Dictionary<FxTypeId, FxEffectConfig> _effectConfigs;
        
        public GameStaticData GameConfig => _gameStaticData;
        public AudioStaticData AudioConfig => _audioStaticData;
        public BalanceStaticData Balance => _balanceStaticData;
        public PlayerStaticData PlayerConfig => _playerStaticData;

        public void LoadData()
        {
            _gameStaticData = Resources
                .Load<GameStaticData>(GameConfigPath);

            _audioStaticData = Resources
                .Load<AudioStaticData>(AudioStaticDataPath);

            _balanceStaticData = Resources
                .Load<BalanceStaticData>(GameBalancePath);

            _playerStaticData = Resources
                .Load<PlayerStaticData>(PlayerConfigPath);

            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowsStaticDataPath)
                .Configs.ToDictionary(x => x.WindowTypeId, x => x);
            
            _effectConfigs = Resources
                .Load<FxEffectStaticData>(FxEffectStaticDataPath)
                .FxEffectContainers.ToDictionary(x => x.FxType, x => x);
        }

        public WindowConfig ForWindow(WindowTypeId windowTypeId) => 
            _windowConfigs[windowTypeId];
        
        public FxEffectConfig ForFxEffect(FxTypeId fxTypeId) => 
            _effectConfigs[fxTypeId];
    }
}