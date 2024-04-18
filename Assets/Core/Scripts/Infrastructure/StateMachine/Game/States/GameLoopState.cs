using UnityEngine;

namespace Infrastructure.StateMachine.Game.States
{
    public class GameLoopState : IState, IGameState, IUpdatable
    {
        public void Enter()
        {
            
        }

        public void Update()
        {
            Debug.Log("1");
        }

        public void Exit()
        {
            
        }
    }
}