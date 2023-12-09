using CameraControll;
using Infrastructure.Services.AudioService;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.Factories.UIFactory;
using Infrastructure.Services.StaticData;
using UnityEngine;
using Controller.Keyboard;
using Entities.MovableEntity.Type;
using Points;

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
            var spawnPointPlayer = GetPlayerPoint();
            var player = InitPlayer(spawnPointPlayer.transform.position);
            InitPlayerController(player);

            InitCameraFollower(player.transform);
        }

        private PlayerPoint GetPlayerPoint() =>
            Object.FindObjectOfType<PlayerPoint>();
        private Player InitPlayer(Vector3 spawnPoint)
        {
            var player = _gameFactory.CreatePlayer(spawnPoint);
            player.Initialize();
            player.InitConfig(_staticDataService.PlayerConfig.MoveSpeed,_staticDataService.PlayerConfig.JumpHeight);
            return player;
        }
        private void InitPlayerController(IConrollable conrollable)
        {
            var controllers = Object.FindObjectOfType<PlayerController>();
            controllers.Initialize(conrollable);
        }

        private void InitCameraFollower(Transform targetTransform)
        {
            var goCamera = Camera.main.gameObject;

            var follower = goCamera.AddComponent<FollowerUpdate>();
            
            follower.Initialize(targetTransform);
        }
    }
}