using Core.Scripts.AIEngines.Healths;
using UnityEngine;

namespace Core.Scripts.AIEngines.Entities.Players
{
    public class Player : MonoBehaviour
    {
        [Space(10)] [Header("Presenters")]
        [SerializeField] private Health _health;
        [Space(10)] [Header("Viewer")]
        [SerializeField] private PlayerHealthViewer _playerHealthViewer;
        [SerializeField] private PlayerAnimatorViewer _playerAnimatorViewer;
        [SerializeField] private PlayerMover _playerMover;
        
        public void Initialize()
        {
            _playerMover.Initialize();
            
            _health.Initialize(100);
            _playerHealthViewer.Initialize(_health);
            
            _playerAnimatorViewer.Initialize(_playerMover);
        }

        public void Dispose()
        {
            _playerAnimatorViewer.Dispose();
        }
        
        public PlayerMover PlayerMover => _playerMover;
    }
}