using Core.Scripts.AIEngines.Entities.Items.Weapons;
using UnityEngine;

namespace Core.Scripts.AIEngines.Entities.Players
{
    public class PlayerAttacker : MonoBehaviour
    {
        [SerializeField] private PlayerAnimatorViewer _playerAnimatorViewer;
        [SerializeField] private Weapon _weapon;

        private bool _isAttacking = false;
        
        public void Initialize()
        {
            _playerAnimatorViewer.TriggeredAttackEvent += OnAttacked;
        }

        public void Dispose()
        {
            _playerAnimatorViewer.TriggeredAttackEvent -= OnAttacked;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartAttack();
            }
        }
        
        private void StartAttack()
        {
            if (_isAttacking) 
                return;
            
            _isAttacking = true;
            _playerAnimatorViewer.PlayAttackAnimation();
        }
        
        private void OnAttacked(string triggerName)
        {
            if (triggerName == "end_attack")
            {
                _isAttacking = false;
                _weapon.Attack();
            }
        }
    }
}