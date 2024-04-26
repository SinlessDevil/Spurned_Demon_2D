using System;
using Entities.MovableEntity.Type;
using Points;

namespace Infrastructure.Services.PlayerServices
{
    public class PlayerService : IPlayerService
    {
        private bool _hasPlayer;
        
        public event Action<Player> OnPlayerAdded;
        public event Action<Player> OnPlayerRemoved;
        
        public Player Player { get; private set; }

        public Point SpawnPoint { get; private set; }

        public bool HasPlayer => _hasPlayer && Player != null;

        public void SetSpawnPoint(Point spawnPoint)
        {
            SpawnPoint = spawnPoint
                ? spawnPoint
                : throw new ArgumentNullException(nameof(spawnPoint));
        }
        
        public void SetPlayer(Player player)
        {
            Player = player
                ? player
                : throw new ArgumentNullException(nameof(player));

            _hasPlayer = true;
            OnPlayerAdded?.Invoke(Player);
        }
        public void Cleanup()
        {
            Player player = Player;

            Player = null;
            _hasPlayer = false;
            
            if(player != null)
                OnPlayerRemoved?.Invoke(player);
        }
    }
}