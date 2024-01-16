using System;
using Infrastructure.Services.AppInfo.Abstractions;
using Infrastructure.Services.PersistenceProgress;
using Infrastructure.Services.PersistenceProgress.Analytic;
using Infrastructure.Services.PersistenceProgress.Player;
using Infrastructure.Services.SaveLoad;
using Application = Infrastructure.Services.PersistenceProgress.Analytic.Application;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadProgressState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IPersistenceProgressService _progressService;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAppInfoService _appInfo;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progressService, 
            ISceneLoader sceneLoader, IAppInfoService appInfo, ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _sceneLoader = sceneLoader;
            _appInfo = appInfo;
            _saveLoadService = saveLoadService;
        }

        public void Enter(string payload)
        {
            LoadOrCreatePlayerData();
            LoadOrCreateAnalyticsData();
            _stateMachine.Enter<BootstrapAnalyticState, string>(payload);
        }

        public void Exit()
        {
            
        }

        private void LoadOrCreateAnalyticsData()
        {
            string id = Guid.NewGuid().ToString();
            long unixTimeMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            _progressService.AnalyticsData = new AnalyticsData(id)
            {
                SessionAmount = 0,
                FirstLoadTimestamp = unixTimeMilliseconds,
                Application = new Application()
                {
                    Version = _appInfo.AppVersion(),
                    UnityVersion = _appInfo.UnityVersion(),
                    BundleID = _appInfo.BundleId()
                },
                CurrentSession = new Session()
            };
        }

        private PlayerData LoadOrCreatePlayerData() =>
            _progressService.PlayerData =
                _saveLoadService.LoadProgress()
                ?? CreatePlayerData();
        private PlayerData CreatePlayerData()
        {
            PlayerData playerData = new PlayerData
            {
            };
            return playerData;
        }
    }
}