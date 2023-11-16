using Infrastructure.Services.Analytics.Events.Behaviour;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.AppInfo.Abstractions;
using Infrastructure.Services.PersistenceProgress.Analytic;
using Infrastructure.Services.PersistenceProgress;
using Infrastructure.Services.Random;
using Infrastructure.Services.FPSMeters;

namespace Infrastructure.StateMachine.Game.States
{
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
}