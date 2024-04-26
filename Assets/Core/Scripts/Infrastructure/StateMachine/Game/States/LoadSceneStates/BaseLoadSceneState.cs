using Infrastructure.Services.AudioService;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.StaticData;
using Zenject;

namespace Infrastructure.StateMachine.Game.States.LoadSceneStates
{
    public abstract class BaseLoadSceneState : IPayloadedState<string>, IGameState
    {
        protected readonly ISceneLoader _sceneLoader;
        protected readonly ILoadingCurtain _loadingCurtain;
        protected readonly IUIFactory _uiFactory;
        protected readonly IStateMachine<IGameState> _gameStateMachine;
        protected readonly IGameFactory _gameFactory;
        protected readonly IStaticDataService _staticDataService;
        protected readonly IAudioClipsService _audioClipsService;

        [Inject]
        public BaseLoadSceneState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain, 
            IUIFactory uiFactory, IGameFactory gameFactory, IStaticDataService staticDataService, IAudioClipsService audioClipsService)
        {
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
            _audioClipsService = audioClipsService;
        }

        public virtual void Enter(string payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(payload, OnLevelLoad);
        }
        public virtual void Exit()
        {
            _loadingCurtain.Hide();
        }

        protected virtual void OnLevelLoad()
        {
            InitGameWorld();

            _gameStateMachine.Enter<LoadLocalizationState>();
        }

        protected abstract void InitGameWorld();

        #region Init UI
        private void InitUIRoot()
        {
            _uiFactory.CreateUiRoot();
        }
        #endregion
    }
}