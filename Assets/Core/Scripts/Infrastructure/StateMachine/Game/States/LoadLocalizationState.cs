using Infrastructure.Services.LocalizationService;
using UnityEngine;
using UI.Hudes;
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
            InitSaveLanguage();

            var hud = FindHud();

            var lozalizes = InitLocalizes();

            InitLocalizeMenu(hud, lozalizes);

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitSaveLanguage()
        {
            if (_localeService.CurrentLanguageHasBeenSet == false)
            {
                _localeService.CurrentLanguageHasBeenSet = true;

                _localeService.CurrentLanguage = _localeService.SystemPlayerLanguage.ToString();
                _localeService.SystemPlayerLanguage = _localeService.SystemPlayerLanguage;
            }
        }
        private Hud FindHud() => Object.FindObjectOfType<Hud>();
        private Localize[] InitLocalizes()
        {
            Localize[] allLocalizes = GameObject.FindObjectsOfType<Localize>(true);

            foreach (var lozalize in allLocalizes)
            {
                lozalize.Initilize();
            }

            return allLocalizes;
        }

        private void InitLocalizeMenu(Hud hud, Localize[] localizes)
        {
            if(hud != null) 
                hud.localizeMenu.Initialize(localizes);
        }

        public void Exit()
        {

        }
    }
}