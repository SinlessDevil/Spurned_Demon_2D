using Infrastructure.Services.Factories.Items;
using UnityEngine;
using Zenject;

namespace Core.Scripts.AIEngines.Entities.Items.Weapons
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponHolder;

        private Weapon _weapon;
        
        private IItemFactory _itemFactory;
        
        [Inject]
        public virtual void Constructor(IItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }
        
        public Weapon GetWeapon() => _weapon;
        
        protected void EquipWeapon(string weaponid)
        {
            UnequipWeapon();
            
            _weapon = _itemFactory.CreateWeapon(weaponid);

            if (_weapon == null)
            {
                Debug.LogError($"Weapon not found {weaponid}");
            }
            
            _weapon.transform.SetParent(_weaponHolder.transform);
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.transform.localRotation = Quaternion.identity;
        }
        
        protected void UnequipWeapon()
        {
            if(_weapon == null) 
                return;
            
            Destroy(_weapon.gameObject);
            _weapon = null;
        }
    }
}