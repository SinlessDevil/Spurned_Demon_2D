using DebuggerOptions;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.AppInfo;
using Infrastructure.Services.AppInfo.Abstractions;
using Infrastructure.Services.AudioService;
using Infrastructure.Services.Coroutines;
using Infrastructure.Services.DeviceData;
using Infrastructure.Services.DeviceData.Abstractions;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.Finish;
using Infrastructure.Services.Finish.Lose;
using Infrastructure.Services.Finish.Win;
using Infrastructure.Services.FPSMeters;
using Infrastructure.Services.FxEffect;
using Infrastructure.Services.LocalizationService;
using Infrastructure.Services.PersistenceProgress;
using Infrastructure.Services.Random;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Timer;
using Infrastructure.Services.Window;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.LoadSceneStates;
using UnityEngine;
using Zenject;
using Application = UnityEngine.Application;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingCurtain _curtain;
        [SerializeField] private TimeService _timeService;

        private RuntimePlatform Platform => Application.platform;

        public override void InstallBindings()
        {
            Debug.Log("Installer");

            BindMonoServices();
            BindServices();
            BindGameStateMachine();
            InitializeDebugger();
            MakeInitializable();
        }
        
        public void Initialize() => BootstrapGame();
        private void BindMonoServices()
        {
            Container.Bind<ICoroutineRunner>().FromMethod(() => Container.InstantiatePrefabForComponent<ICoroutineRunner>(_coroutineRunner)).AsSingle();
            Container.Bind<ILoadingCurtain>().FromMethod(() => Container.InstantiatePrefabForComponent<ILoadingCurtain>(_curtain)).AsSingle();
            Container.Bind<ITimeService>().FromMethod(() => Container.InstantiatePrefabForComponent<ITimeService>(_timeService)).AsSingle();
            
            BindSceneLoader();
        }

        private void BindServices()
        {
            BindStaticDataService();
            
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<WindowService>().AsSingle();
            Container.BindInterfacesTo<PersistenceProgressService>().AsSingle(); 
            Container.BindInterfacesTo<FPSMeter>().AsSingle();
            Container.BindInterfacesTo<RandomService>().AsSingle();
            Container.BindInterfacesTo<GameFactory>().AsSingle();
            Container.BindInterfacesTo<FinishService>().AsSingle();
            Container.BindInterfacesTo<WinService>().AsSingle();
            Container.BindInterfacesTo<LoseService>().AsSingle();
            Container.BindInterfacesTo<AudioClipsService>().AsSingle();
            Container.BindInterfacesTo<LocaleService>().AsSingle();
            Container.BindInterfacesTo<CoroutineService>().AsSingle();
            Container.BindInterfacesTo<FxEffectService>().AsSingle();
            
            BindEnrichedAnalyticService<AnalyticService>();
            BindDeviceDataService(); 
            BindAppInfoService();
        }
        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            
            BindGameStates();
        }

        private void InitializeDebugger()
        {
            SRDebug.Init();
            SRDebug.Instance.AddOptionContainer(Container.Instantiate<SROptionsContainer>());
        }
        private void MakeInitializable() => Container.Bind<IInitializable>().FromInstance(this);

        private void BindSceneLoader()
        {
            ISceneLoader sceneLoader = new SceneLoader(Container.Resolve<ICoroutineRunner>());
            Container.Bind<ISceneLoader>().FromInstance(sceneLoader).AsSingle();
        }
        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.LoadData();
            Container.Bind<IStaticDataService>().FromInstance(staticDataService).AsSingle();
        }

        private void BindAppInfoService()
        {
            Container
                .Bind<IAppInfoService>()
                .FromMethod(SelectImplementation<IAppInfoService,
                    AndroidAppInfoService,
                    IOSAppInfoService,
                    EditorAppInfoService>)
                .AsSingle();
        }
        private void BindDeviceDataService()
        {
            Container
                .Bind<IDeviceDataService>()
                .FromMethod(SelectImplementation<IDeviceDataService,
                    AndroidDeviceDataService,
                    IOSDeviceDataService,
                    EditorDeviceDataService>)
                .AsSingle();
        }
        private void BindEnrichedAnalyticService<TAnalytic>() where TAnalytic : IAnalyticService
        {
            Container.Bind<IAnalyticService>()
                .FromMethod(() => Container
                    .Instantiate<AnalyticEnrichService>(new object[]
                    {
                        Container.Instantiate<TAnalytic>()
                    }))
                .AsSingle();
        }
        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<BootstrapAnalyticState>().AsSingle();
            Container.Bind<BootstrapAudioState>().AsSingle();
            Container.Bind<DelegateStatesForSceneState>().AsSingle();
            Container.Bind<LoadGameState>().AsSingle();
            Container.Bind<LoadMenuState>().AsSingle();
            Container.Bind<LoadLocalizationState>().AsSingle();   
            Container.Bind<GameLoopState>().AsSingle();
        }
        private void BootstrapGame() => 
            Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();
        private TOut SelectImplementation<TOut, TAndroid, TIos, TEditor>() 
            where TAndroid: TOut 
            where TIos: TOut 
            where TEditor: TOut
        {
            TOut implementation = Platform switch
            {
                RuntimePlatform.Android => Container.Instantiate<TAndroid>(),
                RuntimePlatform.IPhonePlayer => Container.Instantiate<TIos>(),
                _ => Container.Instantiate<TEditor>()
            };

            return implementation;
        }
    }
}