using Infrastructure.Services.LocalizationService;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadLocalizationState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ILocaleService _localeService;

        [Inject]
        public LoadLocalizationState(IStateMachine<IGameState> gameStateMachine, ILocaleService localeService)
        {
            _gameStateMachine = gameStateMachine;
            _localeService = localeService;
        }

        public void Enter()
        {
            var lozalizes = InitLocalizes();

            InitLocolizeMenu(lozalizes);

            _gameStateMachine.Enter<GameLoopState>();
        }

        private Localize[] InitLocalizes()
        {
            var lozalizes = Object.FindObjectsOfType<Localize>();

            foreach (var lozalize in lozalizes)
            {
                lozalizes.Initialize();
            }

            return lozalizes;
        }
        private void InitLocolizeMenu(Localize[] localizes)
        {
            var locolizeMenu = Object.FindObjectOfType<LocolizeMenu>();

            if(locolizeMenu != null) 
            {
                locolizeMenu.Initialize(localizes);
            }
        }

        public void Exit()
        {

        }
    }
}