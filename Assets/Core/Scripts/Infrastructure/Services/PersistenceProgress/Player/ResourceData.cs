using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Wallet;

namespace Infrastructure.Services.PersistenceProgress.Player
{
    [Serializable]
    public class ResourceData
    {
        public List<CurrencySave> Currencies = new();

        public CurrencySave GetOrCreateCurrencySave(CurrencyType currencyType)
        {
            foreach (var currency in Currencies.Where(currency => currency.CurrencyType == currencyType))
            {
                return currency;
            }

            var currencySave = new CurrencySave
            {
                CurrencyType = currencyType,
                Amount = 0
            };
            
            Currencies.Add(currencySave);

            return currencySave;
        }
    }
}