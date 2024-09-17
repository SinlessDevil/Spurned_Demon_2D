using Infrastructure.Services.StaticData;
using UnityEngine;
using Core.Scripts.AIEngines.Entities.Items.Weapons;
using Zenject;

namespace Core.Scripts.AIEngines.Entities.Players
{
    public class PlayerAttacker : MonoBehaviour
    {
        [SerializeField] private PlayerAnimatorViewer _playerAnimatorViewer;
        [SerializeField] private WeaponHolder _weaponHolder;

        private bool _isAttacking = false;
        
        private IStaticDataService _staticDataService;
        
        [Inject]
        public void Constructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Initialize() => _playerAnimatorViewer.TriggeredAttackEvent += OnAttacked;
        public void Dispose() => _playerAnimatorViewer.TriggeredAttackEvent -= OnAttacked;
        
        private void Update()
        {
            if (Input.GetKeyDown(_staticDataService.KeyWordsConfig.AttackKey))
            {
                StartAttack();
            }
        }
        
        private void StartAttack()
        {
            if(_weaponHolder.GetWeapon() == null)
                return;
            
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
                _weaponHolder.GetWeapon().Attack();
            }
        }
    }
}