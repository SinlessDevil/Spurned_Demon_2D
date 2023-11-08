using UnityEngine;
using Scripts.Infrastructure.Services.Factories.GameFactory;
using Scripts.Infrastructure.Services.Factories.UIFactory;
using Scripts.Infrastructure.Services.StaticData.Level;
using Scripts.Infrastructure.StateMachine.Game;
using Scripts.StaticData;

namespace Scripts.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly LevelStaticData _levelStaticData;

        public LoadLevelState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader, 
            IGameFactory gameFactory, IUIFactory uiFactory, ILevelStaticDataService levelStaticDataService)
        {
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _levelStaticData = levelStaticDataService.GameConfig();
        }

        public void Enter(string payload)
        {
            Debug.Log("Enter Loading Level");

            _sceneLoader.Load(payload, OnLevelLoad);
        }

        public void Exit()
        {
            Debug.Log("Exit Loading Level");
        }

        protected virtual void OnLevelLoad()
        {
            InitGameWorld();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            _uiFactory.CreateUiRoot();
        }
    }
}