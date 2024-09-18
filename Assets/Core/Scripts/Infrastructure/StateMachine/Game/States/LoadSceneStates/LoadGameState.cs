using Infrastructure.Services.AudioService;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Inputs;
using Infrastructure.Services.PlayerServices;
using Infrastructure.StaticData;
using UnityEngine;
using Core.Scripts.AIEngines.Entities.Players;
using CameraControll;
using GameController;
using Points;
using UI.Hudes;

namespace Infrastructure.StateMachine.Game.States.LoadSceneStates
{
    public class LoadGameState : BaseLoadSceneState
    {
        private PCInputDevice _pcInputDevice;
        
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
            PlayerMoveController playerMoveController) : base(gameStateMachine, sceneLoader, loadingCurtain, 
            uiFactory, gameFactory, staticDataService, audioClipsService)
        {
            _inputService = inputService;
            _playerService = playerService;
            _playerMoveController = playerMoveController;
        }

        protected override void InitGameWorld()
        {
            var gameHud = InitGameHud();
            InitInput(gameHud);
            
            ActiveControllers();
            var player = InitPlayer();
            InitCameraFollower(player.transform);
        }
        private GameHud InitGameHud()
        {
           var gamehud = _uiFactory.CreateGameHud();
           return gamehud;
        }
        private void InitInput(GameHud gamehud)
        {
            if (_staticDataService.InputConfig.InputType == InputType.Keyboard)
            {
                _pcInputDevice = new PCInputDevice();
                _inputService.SetInputDevice(_pcInputDevice);
                gamehud.ToggleJoyStick(false);
            }
            else
            {
                _inputService.SetInputDevice(gamehud.Joystick);
                gamehud.ToggleJoyStick(true);
            }
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
            _playerService.SetSpawnPoint(spawnPointPlayer);
            return player;
        }
        private void InitCameraFollower(Transform targetTransform)
        {
            var goCamera = Camera.main.gameObject;
            var follower = goCamera.AddComponent<FollowerFixedUpdate>();
            follower.Initialize(targetTransform,_staticDataService.Balance.Camera.Offset, _staticDataService.Balance.Camera.Smoothing);
        }
    }
}