using Infrastructure.Services.AudioService;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.StaticData;

namespace Infrastructure.StateMachine.Game.States.LoadSceneStates
{
    public class LoadGameState : BaseLoadSceneState
    {
        public LoadGameState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain, IUIFactory uiFactory, 
            IGameFactory gameFactory, IStaticDataService staticDataService, IAudioClipsService audioClipsService) : 
            base(gameStateMachine, sceneLoader, loadingCurtain, uiFactory, gameFactory, staticDataService, audioClipsService)
        {

        }

        protected override void InitGameWorld()
        {

        }
    }
}