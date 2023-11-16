using Infrastructure.Services.AudioService;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.StaticData;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IAudioClipsService _audioClipsService;

        [Inject]
        public LoadLevelState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain, 
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

        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(payload, OnLevelLoad);
        }
        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        protected virtual void OnLevelLoad()
        {
            InitGameWorld();

            _gameStateMachine.Enter<LoadLocalizationState>();
        }

        private void InitGameWorld()
        {
            InitUIRoot();

            _audioClipsService.PlayClip(TypeSound.Menu); // Test
        }

        #region Init UI
        private void InitUIRoot()
        {
            _uiFactory.CreateUiRoot();
        }
        #endregion
    }
}