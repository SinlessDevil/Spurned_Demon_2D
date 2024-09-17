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
        [SerializeField] private PlayerAttacker _playerAttacker;
        [SerializeField] private PlayerWeaponHolder _playerWeaponHolder;
        
        private bool _isInitialized = false;
        
        public PlayerMover PlayerMover => _playerMover;
        
        public void Initialize()
        {
            if (_isInitialized)
            {
                Dispose();
            }
            
            _playerMover.Initialize();
            _health.Initialize(100);
            _playerHealthViewer.Initialize(_health);
            _playerAnimatorViewer.Initialize(_playerMover);
            _playerAttacker.Initialize();
            _playerWeaponHolder.Initialize();
            
            _isInitialized = true;
        }

        private void Dispose()
        {
            _playerAnimatorViewer.Dispose();
            _playerHealthViewer.Dispose();
            _playerAttacker.Dispose();
            
            _isInitialized = false;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}