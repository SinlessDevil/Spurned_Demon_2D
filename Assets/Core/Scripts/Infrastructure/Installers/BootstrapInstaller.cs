using DebuggerOptions;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.States;
using Services.Analytics;
using Services.AppInfo;
using Services.AppInfo.Abstractions;
using Services.AudioService;
using Services.DeviceData;
using Services.DeviceData.Abstractions;
using Services.Factories.Game;
using Services.Factories.UIFactory;
using Services.PersistenceProgress;
using Services.Random;
using Services.StaticData;
using Services.Window;
using UnityEngine;
using Zenject;
using Application = UnityEngine.Application;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingCurtain _curtain;

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
            Container.BindInterfacesTo<AudioClipsService>().AsSingle();

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
            Container.Bind<LoadLevelState>().AsSingle();
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