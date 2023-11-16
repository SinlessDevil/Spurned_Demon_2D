using Infrastructure.StateMachine.Game.States.LoadSceneStates;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class DelegateStatesForSceneState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;

        private const string MenuScene = "Menu";
        private const string GameScene = "GamePlay";

        [Inject]
        public DelegateStatesForSceneState(IStateMachine<IGameState> gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(string payload)
        {
            if(payload == MenuScene)
            {
                _gameStateMachine.Enter<LoadMenuState, string>(payload);
                return;
            }

            if (payload == GameScene)
            {
                _gameStateMachine.Enter<LoadGameState, string>(payload);
                return;
            }

            Debug.LogError($"{payload} Scene was not found in stateMappings !");

            LoadBaseState(payload);
        }

        private void LoadBaseState(string payload)
        {
            _gameStateMachine.Enter<BaseLoadSceneState, string>(payload);
        }

        public void Exit()
        {

        }
    }
}