using Infrastructure.Services.LocalizationService;
using UnityEngine;
using UI;
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
            var hud = FindHud();

            var lozalizes = InitLocalizes();

            InitLocolizeMenu(hud, lozalizes);

            _gameStateMachine.Enter<GameLoopState>();
        }
        private Hud FindHud() => Object.FindObjectOfType<Hud>();

        private Localize[] InitLocalizes()
        {
            var lozalizes = Resources.FindObjectsOfTypeAll<Localize>();

            foreach (var lozalize in lozalizes)
            {
                lozalize.Initilize();
            }

            return lozalizes;
        }
        private void InitLocolizeMenu(Hud hud,Localize[] localizes) => hud.localizeMenu.Initialize(localizes);

        public void Exit()
        {

        }
    }
}