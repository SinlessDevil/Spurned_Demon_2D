using CameraControll;
using Infrastructure.Services.AudioService;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Input;
using Infrastructure.Services.PlayerServices;
using UnityEngine;
using Entities.MovableEntity.Type;
using GameController;
using Points;

namespace Infrastructure.StateMachine.Game.States.LoadSceneStates
{
    public class LoadGameState : BaseLoadSceneState
    {
        private readonly IInputService _inputService;
        private readonly IPlayerService _playerService;
        private readonly PlayerMoveController _playerMoveController;
        
        public LoadGameState(
            IStateMachine<IGameState> gameStateMachine, 
            ISceneLoader sceneLoader, 
            ILoadingCurtain loadingCurtain, 
            IUIFactory uiFactory, 
            IGameFactory gameFactory, 
            IStaticDataService staticDataService, 
            IAudioClipsService audioClipsService,
            IInputService inputService,
            IPlayerService playerService,
            PlayerMoveController playerMoveController) : 
            base(gameStateMachine, sceneLoader, loadingCurtain, uiFactory, gameFactory, staticDataService, audioClipsService)
        {
            _inputService = inputService;
            _playerService = playerService;
            _playerMoveController = playerMoveController;
        }

        protected override void InitGameWorld()
        {
            InitGameHud();

            ActiveControllers();
            var player = InitPlayer();
            InitCameraFollower(player.transform);
        }

        private void InitGameHud()
        {
           var gamehud = _uiFactory.CreateGameHud();
           _inputService.SetInputDevice(gamehud.Joystick);
        }

        private void ActiveControllers()
        {
            _playerMoveController.Activate();
        }
        private PlayerPoint GetPlayerPoint() => Object.FindObjectOfType<PlayerPoint>();
        private Player InitPlayer()
        {
            var spawnPointPlayer = GetPlayerPoint();
            
            var player = _gameFactory.CreatePlayer(spawnPointPlayer.transform.position);
            _playerService.SetPlayer(player);
            player.Initialize();
            player.InitConfig(_staticDataService.PlayerConfig.MoveSpeed, _staticDataService.PlayerConfig.JumpHeight);
            return player;
        }
        private void InitCameraFollower(Transform targetTransform)
        {
            var goCamera = Camera.main.gameObject;

            var follower = goCamera.AddComponent<FollowerUpdate>();
            
            follower.Initialize(targetTransform);
        }
        
        public override void Exit()
        {
            base.Exit();
            _inputService.Cleanup();
            _playerService.Cleanup();
        }
    }
}