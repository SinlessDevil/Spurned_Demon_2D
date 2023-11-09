using System.ComponentModel;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using JetBrains.Annotations;
using UnityEngine;

namespace DebuggerOptions
{
    public class SROptionsContainer
    {
        private readonly IStateMachine<IGameState> _stateMachine;

        public SROptionsContainer(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        #region Debug Methos
        [Category("Debug"), UsedImplicitly]
        public void PrintMessageToConsole() => Debug.Log("SROptions work's perfect!");
        #endregion

        #region Cheats Methods
        [Category("Cheats"), UsedImplicitly]
        public void ToggleTimeAccelerationDouble() => Time.timeScale = (Time.timeScale > 1.0f) ? 1.0f : 2.0f;
        [Category("Cheats"), UsedImplicitly]
        public void ToggleTimeAccelerationTriple() => Time.timeScale = (Time.timeScale > 1.0f) ? 1.0f : 3.0f;
        #endregion
    }
}