using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.LoadSceneStates;
using Zenject;

namespace Infrastructure.StateMachine.Game
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(DiContainer container) : base(container) {}

        protected override Dictionary<Type, Func<IExitable>> BuildStatesRegister(DiContainer container)
        {
            return new Dictionary<Type, Func<IExitable>>()
            {
                [typeof(BootstrapState)] = container.Resolve<BootstrapState>,
                [typeof(LoadProgressState)] = container.Resolve<LoadProgressState>,
                [typeof(BootstrapAnalyticState)] = container.Resolve<BootstrapAnalyticState>,
                [typeof(BootstrapAudioState)] = container.Resolve<BootstrapAudioState>,
                [typeof(DelegateStatesForSceneState)] = container.Resolve<DelegateStatesForSceneState>,
                [typeof(LoadMenuState)] = container.Resolve<LoadMenuState>,
                [typeof(LoadGameState)] = container.Resolve<LoadGameState>,
                [typeof(LoadLocalizationState)] = container.Resolve<LoadLocalizationState>,
                [typeof(GameLoopState)] = container.Resolve<GameLoopState>,          
            };
        }
    }
}