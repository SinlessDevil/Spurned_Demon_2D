using Controllers;
using GameController;
using Infrastructure.StateMachine.Game.States.LoadSceneStates;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class DelegateStatesForSceneState : IPayloadedState<string>, IGameState
    {
        private const string MenuScene = "Menu";
        private const string GameScene = "GamePlay";
        
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IGameController _gameController;

        [Inject]
        public DelegateStatesForSceneState(IStateMachine<IGameState> gameStateMachine,
            PlayerMoveController playerMoveController)
        {
            _gameStateMachine = gameStateMachine;
            _gameController = new CompositeController(
                playerMoveController
            );
        }

        public void Enter(string payload)
        {
            switch (payload)
            {
                case MenuScene:
                    _gameStateMachine.Enter<LoadMenuState, string>(payload);
                    return;
                case GameScene:
                    _gameStateMachine.Enter<LoadGameState, string>(payload);
                    return;
                default:
                    Debug.LogError($"{payload} Scene was not found in stateMappings !");
                    LoadBaseState(payload);
                    break;
            }
        }

        private void LoadBaseState(string payload)
        {
            _gameStateMachine.Enter<BaseLoadSceneState, string>(payload);
        }

        public void Exit()
        {
            _gameController.Deactivate();
        }
    }
}