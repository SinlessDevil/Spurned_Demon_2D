using System;

namespace Core.Scripts.AIEngines.Healths
{
    public interface IHealth
    {
        event Action<int> ChangedHealthEvent;
        event Action<int> AppliedDamageEvent;
        event Action<int> AppliedHealEvent;
        event Action DiedEvent;
        int GetMaxHealth { get; }
        void ApplyDamage(int damage);
        void ApplyHeal(int heal);
    }
}