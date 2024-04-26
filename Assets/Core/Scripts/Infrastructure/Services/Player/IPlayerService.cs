using System;
using Entities.MovableEntity.Type;
using Points;

namespace Infrastructure.Services.PlayerServices
{
    public interface IPlayerService
    {
        Player Player { get; }
        Point SpawnPoint { get; }
        bool HasPlayer { get; }
        void SetPlayer(Player player);
        void Cleanup();
        event Action<Player> OnPlayerAdded;
        event Action<Player> OnPlayerRemoved;
        void SetSpawnPoint(Point spawnPoint);
    }
}