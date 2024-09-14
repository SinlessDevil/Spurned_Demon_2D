using System;
using Infrastructure.Services.Wallet;

namespace Infrastructure.Services.PersistenceProgress.Player
{
    [Serializable]
    public class CurrencySave
    {
        public CurrencyType CurrencyType;
        public int Amount;
    }
}