using UnityEngine;
using Entities.MovableEntity.Type;

namespace Infrastructure.Services.Factories.Game
{
    public interface IGameFactory
    {
        public Player CreatePlayer(Vector3 startPos);
    }
}