using Core.Scripts.AIEngines.Entities.Players;
using Infrastructure.StaticData;
using UnityEngine;

namespace Infrastructure.Services.Factories.Game
{
    public interface IGameFactory
    {
        public Player CreatePlayer(Vector3 startPos);
        public ParticleSystem CreateFxEffect(FxTypeId fxTypeId, Vector3 spawnPos);
    }
}