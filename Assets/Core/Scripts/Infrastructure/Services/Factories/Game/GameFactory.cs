using Infrastructure.Services.StaticData;
using UnityEngine;
using Entities.MovableEntity.Type;
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
            var player = Instantiate(_staticData.PlayerConfig.Prefab, spawnPosition, Quaternion.identity,null);
            return player.GetComponent<Player>();
        }
    }
}