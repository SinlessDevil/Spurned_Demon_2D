using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Services.Analytics;
using Services.Analytics.Events.Behaviour;
using Services.AppInfo.Abstractions;
using Services.PersistenceProgress;
using Services.PersistenceProgress.Analytic;
using Services.PersistenceProgress.Player;
using Services.Random;
using UnityEngine;
using Zenject;
using Application = Services.PersistenceProgress.Analytic.Application;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadProgressState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IPersistenceProgressService _progressService;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAppInfoService _appInfo;

        public LoadProgressState(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progressService, ISceneLoader sceneLoader, IAppInfoService appInfo)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _sceneLoader = sceneLoader;
            _appInfo = appInfo;
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
            _progressService.PlayerData = new PlayerData();
    }
    
    public class BootstrapAnalyticState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly AnalyticsData _analyticsData;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAppInfoService _appInfoService;
        private readonly IAnalyticService _analyticService;
        private readonly IRandomService _randomService;
        private readonly IFPSMeter _fpsMeter;

        public BootstrapAnalyticState(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progressService, ISceneLoader sceneLoader, IAppInfoService appInfoService, IAnalyticService analyticService, IRandomService randomService, IFPSMeter fpsMeter)
        {
            _stateMachine = stateMachine;
            _analyticsData = progressService.AnalyticsData;
            _sceneLoader = sceneLoader;
            _appInfoService = appInfoService;
            _analyticService = analyticService;
            _randomService = randomService;
            _fpsMeter = fpsMeter;
        }

        public void Enter(string payload)
        {
            _fpsMeter.Begin();
            
            RefreshAnalyticsData();
            TrySendFirstSessionEvent();
            SendStartSessionEvent();

            if (IsVersionChanged()) 
                RefreshVersionNotified();

            _stateMachine.Enter<BootstrapAudioState, string>(payload);
        }

        private void RefreshAnalyticsData()
        {
            _analyticsData.SessionAmount++;
            _analyticsData.CurrentSession.Id = _randomService.GenerateId();
        }

        private void TrySendFirstSessionEvent()
        {
            if (_analyticsData.IsFirstSession)
                SendFirstSessionEvent();
        }

        private void SendFirstSessionEvent() => 
            _analyticService.Send(new FirstOpen(_analyticsData.FirstLoadTimestamp));

        private void SendStartSessionEvent() => 
            _analyticService.Send(new StartSession(_analyticsData.SessionAmount, _analyticsData.CurrentSession.Id));

        private bool IsVersionChanged() => 
            _analyticsData.Application.Version != _appInfoService.AppVersion();

        private void RefreshVersionNotified()
        {
            _analyticsData.Application.Version = _appInfoService.AppVersion();
            _analyticService.Send(new AppUpdate(_analyticsData.Application.Version));
        }

        public void Exit()
        {
            
        }
    }

    public interface IFPSMeter
    {
        void Begin();
        void Break();
    }

    public class FPSMeter: IFPSMeter
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPersistenceProgressService _progressService;
        
        private Queue<float> _capturedFrames = new Queue<float>();
        private int _framesCount = 10;
        private Coroutine _coroutine;

        [Inject]
        public FPSMeter(ICoroutineRunner coroutineRunner,IPersistenceProgressService progressService)
        {
            _coroutineRunner = coroutineRunner;
            _progressService = progressService;
        }

        private float AverageFPS => _capturedFrames.Average();
        

        public void Begin()
        {
            _coroutine = _coroutineRunner.StartCoroutine(LoopFPSCheck());
        }

        public void Break()
        {
            _coroutineRunner.StopCoroutine(_coroutine);
        }
        
        private IEnumerator LoopFPSCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                
                CaptureFrame();
                RefreshFPSData();
            }
        }

        private void CaptureFrame()
        {
            if (NeedDequeue())
                _capturedFrames.Dequeue();

            _capturedFrames.Enqueue(CurrentFPS());
        }

        private bool NeedDequeue() => 
            _capturedFrames.Count > _framesCount;

        private float CurrentFPS() => 
            1 / Time.deltaTime;

        private void RefreshFPSData() => 
            _progressService.AnalyticsData.CurrentSession.FPS = AverageFPS;
    }
}