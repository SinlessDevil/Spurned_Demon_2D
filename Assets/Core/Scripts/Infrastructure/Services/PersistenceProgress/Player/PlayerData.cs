using System;

namespace Infrastructure.Services.PersistenceProgress.Player
{
    [Serializable]
    public class PlayerData
    {
        public event Action MoneyChangedEvent;
        public event Action KeyChangedEvent;
        
        public int Money = 0;
        public int Key = 0;
        public void AddMoney(int value) => AddValue(ref Money, value, MoneyChangedEvent);
        public void RemoveMoney(int value) => RemoveValue(ref Money, value, MoneyChangedEvent);
        
        public void AddKey(int value) => AddValue(ref Key, value, KeyChangedEvent);
        public void RemoveKey(int value) => RemoveValue(ref Key, value, KeyChangedEvent);
        
        private void AddValue(ref int currentValue, int addedValue, Action OnValueChangedEvent)
        {
            currentValue += addedValue;
    
            OnValueChangedEvent?.Invoke();
        }
        private void RemoveValue(ref int currentValue, int subtractValue, Action OnValueChangedEvent)
        {
            currentValue -= subtractValue;
    
            if(currentValue < 0)
                currentValue = 0;
    
            OnValueChangedEvent?.Invoke();
        }
    }
}