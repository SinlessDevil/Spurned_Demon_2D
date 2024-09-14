using UnityEngine;

namespace Core.Scripts.AIEngines.Entities.Items.Weapons
{
    public abstract class Weapon : Item
    {
        [SerializeField] protected float _attackRadius = 5f;
        [SerializeField] protected LayerMask _targetLayer;
        [SerializeField] protected Vector3 _centerOffset = Vector3.zero;
        
        protected int _damage;
        
        public void Initialize(int damage)
        {
            _damage = damage;
        }
        
        public abstract void Attack();
    }
}