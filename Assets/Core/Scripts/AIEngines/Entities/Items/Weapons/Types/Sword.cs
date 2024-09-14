using Core.Scripts.AIEngines.Healths;
using UnityEngine;

namespace Core.Scripts.AIEngines.Entities.Items.Weapons.Types
{
    public class Sword : Weapon
    {
        [SerializeField] private float _capsuleHeight = 2f;

        public override void Attack()
        {
            Vector3 center = transform.position + _centerOffset;
            Vector3 point1 = center + Vector3.up * (_capsuleHeight / 2);
            Vector3 point2 = center - Vector3.up * (_capsuleHeight / 2);
            Collider[] hitColliders = Physics.OverlapCapsule(point1, point2, _attackRadius, _targetLayer);
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.gameObject.TryGetComponent<Health>(out var health))
                {
                    health.ApplyDamage(_damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 center = transform.position + _centerOffset;
            Vector3 point1 = center + Vector3.up * (_capsuleHeight / 2);
            Vector3 point2 = center - Vector3.up * (_capsuleHeight / 2);
            Gizmos.DrawWireSphere(point1, _attackRadius);
            Gizmos.DrawWireSphere(point2, _attackRadius);
            Gizmos.DrawLine(point1 + Vector3.forward * _attackRadius, point2 + Vector3.forward * _attackRadius);
            Gizmos.DrawLine(point1 - Vector3.forward * _attackRadius, point2 - Vector3.forward * _attackRadius);
            Gizmos.DrawLine(point1 + Vector3.right * _attackRadius, point2 + Vector3.right * _attackRadius);
            Gizmos.DrawLine(point1 - Vector3.right * _attackRadius, point2 - Vector3.right * _attackRadius);
        }
    }
}