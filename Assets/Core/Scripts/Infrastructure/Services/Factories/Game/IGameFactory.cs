using Infrastructure.StaticData;
using UnityEngine;
using Entities.MovableEntity.Type;

namespace Infrastructure.Services.Factories.Game
{
    public interface IGameFactory
    {
        public Player CreatePlayer(Vector3 startPos);
        public ParticleSystem CreateFxEffect(FxTypeId fxTypeId, Vector3 spawnPos);
    }
}