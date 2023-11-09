using System.Linq;
using System.Collections.Generic;
using StaticData;
using UnityEngine;
using Window;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameConfigPath = "StaticData/Balance/GameConfig";
        private const string GameBalancePath = "StaticData/Balance/Balance";
        private const string WindowsStaticDataPath = "StaticData/WindowsStaticData";

        private GameStaticData _gameStaticData;
        private BalanceStaticData _balanceStaticData;

        private Dictionary<WindowTypeId, WindowConfig> _windowConfigs;

        public GameStaticData GameConfig => _gameStaticData;
        public BalanceStaticData Balance => _balanceStaticData;

        public void LoadData()
        {
            _gameStaticData = Resources
                .Load<GameStaticData>(GameConfigPath);

            _balanceStaticData = Resources
                .Load<BalanceStaticData>(GameBalancePath);

            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowsStaticDataPath)
                .Configs.ToDictionary(x => x.WindowTypeId, x => x);
        }

        public WindowConfig ForWindow(WindowTypeId windowTypeId) => 
            _windowConfigs[windowTypeId];
    }
}