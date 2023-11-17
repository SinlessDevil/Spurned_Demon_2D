using Infrastructure.Services.AudioService;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.StaticData;
using UI.Hudes;

namespace Infrastructure.StateMachine.Game.States.LoadSceneStates
{
    public class LoadMenuState : BaseLoadSceneState
    {
        public LoadMenuState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain, IUIFactory uiFactory, 
            IGameFactory gameFactory, IStaticDataService staticDataService, IAudioClipsService audioClipsService) : 
            base(gameStateMachine, sceneLoader, loadingCurtain, uiFactory, gameFactory, staticDataService, audioClipsService)
        {
        }

        protected override void InitGameWorld()
        {
            var menuHud = InitMenuHud();

            InitSettings(menuHud);

            InitMainMenu(menuHud);

            InitMenuMusic();
        }

        private MenuHud InitMenuHud() => _uiFactory.CreateMenuHud();
        private void InitMenuMusic() => _audioClipsService.PlayClip(TypeSound.Menu);
        private void InitSettings(MenuHud menuHud) => menuHud.Settings.Initialize();
        private void InitMainMenu(MenuHud menuHud) => menuHud.MainMenu.Initialize(menuHud.Settings);
    }
}
