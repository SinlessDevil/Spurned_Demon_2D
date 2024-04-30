using System;
using UnityEngine;

namespace Core.Scripts.AIEngines.Healths
{
    public class Health : MonoBehaviour, IHealth
    {
        private int _maxHealth;
        private int _currentHealth;
        private IHealth _healthImplementation;

        public void Initialize(int mxHealth)
        {
            _maxHealth = mxHealth;
            _currentHealth = _maxHealth;
        }

        public event Action<int> ChangedHealthEvent;
        public event Action<int> AppliedDamageEvent;
        public event Action<int> AppliedHealEvent;
        public event Action DiedEvent;

        public int GetMaxHealth => _maxHealth;
        
        [Sirenix.OdinInspector.Button]
        public void ApplyDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                ChangedHealthEvent?.Invoke(_currentHealth);
                AppliedDamageEvent?.Invoke(damage);
                DiedEvent?.Invoke();
                return;
            }

            ChangedHealthEvent?.Invoke(_currentHealth);
            AppliedDamageEvent?.Invoke(damage);
        }

        [Sirenix.OdinInspector.Button]
        public void ApplyHeal(int heal)
        {
            _currentHealth += heal;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            ChangedHealthEvent?.Invoke(_currentHealth);
            AppliedHealEvent?.Invoke(heal);
        }
    }
}