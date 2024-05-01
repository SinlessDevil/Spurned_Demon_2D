using Core.Scripts.AIEngines.Entities.Players;
using Infrastructure.Services.StaticData;
using UnityEngine;
using Infrastructure.StaticData;
using Zenject;

namespace Infrastructure.Services.Factories.Game
{
    public class GameFactory : Factory, IGameFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;

        public GameFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }

        public Player CreatePlayer(Vector3 spawnPosition)
        {
            var player = Instantiate(Path.PlayerPath, spawnPosition, Quaternion.identity,null).GetComponent<Player>();
            player.PlayerMover.InitConfig(_staticData.PlayerConfig.MoveSpeed, _staticData.PlayerConfig.JumpHeight);
            return player;
        }
        public ParticleSystem CreateFxEffect(FxTypeId fxTypeId,Vector3 spawnPos)
        {
            FxEffectConfig config = _staticData.ForFxEffect(fxTypeId);
            var fxEffect = Instantiate(config.FxPrefab.gameObject, spawnPos, Quaternion.identity,null);
            return fxEffect.GetComponent<ParticleSystem>();
        }
        
        private PathResourcesStaticData Path => _staticData.PathResourcesConfig;
    }
}