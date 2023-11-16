using Infrastructure.Services.StaticData;
using UnityEngine.SceneManagement;

namespace Infrastructure.StateMachine.Game.States
{
    public class BootstrapState : IState, IGameState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticData;
        private readonly ILoadingCurtain _curtain;
        private string _firstSceneName;

        public BootstrapState(IStateMachine<IGameState> stateMachine, ISceneLoader sceneLoader, IStaticDataService staticDataService, ILoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticData = staticDataService;
            _curtain = curtain;
        }

        public void Enter()
        {
            _curtain.Show();
            _firstSceneName = FirstSceneName();
            _sceneLoader.Load(_staticData.GameConfig.InitialScene, OnLevelLoad);
        }

        public void Exit()
        {

        }

        private void OnLevelLoad() => 
            _stateMachine.Enter<LoadProgressState, string>(_firstSceneName);

        private string FirstSceneName()
        {
            string name = _staticData.GameConfig.FirstScene;
            
#if UNITY_EDITOR
            if (_staticData.GameConfig.CanLoadCurrentOpenedScene)
                name = SceneManager.GetActiveScene().name;        
#endif
                return name;
        }
    }
}