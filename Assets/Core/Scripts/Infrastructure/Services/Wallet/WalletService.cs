using System;
using Infrastructure.Services.PersistenceProgress;
using Infrastructure.Services.PersistenceProgress.Player;

namespace Infrastructure.Services.Wallet
{
    public class WalletService : IWalletService
    {
        private readonly IPersistenceProgressService _progressService;

        public WalletService(IPersistenceProgressService progressService)
        {
            _progressService = progressService;
        }

        public event Action<CurrencyType> CurrencyChanged;
        public event Action<CurrencyType, int> CurrencyAdded;
        public event Action<CurrencyType, int> CurrencyRemoved;

        private ResourceData PlayerDataResourceData => _progressService.PlayerData.ResourceData;

        public bool CanWithdraw(Price price) => CanWithdraw(price.CurrencyType, price.Amount);
        public void Withdraw(Price price) => Withdraw(price.CurrencyType, price.Amount);
        public void Add(Price price) => Add(price.CurrencyType, price.Amount);

        public bool CanWithdraw(CurrencyType currencyType, int amount) =>
            GetCurrencySave(currencyType).Amount >= amount;

        public void Withdraw(CurrencyType currencyType, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), $"was {amount}");
            
            GetCurrencySave(currencyType).Amount -= amount;
            
            CurrencyRemoved?.Invoke(currencyType, amount);
            CurrencyChanged?.Invoke(currencyType);
        }

        public void Add(CurrencyType currencyType, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), $"was {amount}");
            
            GetCurrencySave(currencyType).Amount += amount;
            
            CurrencyAdded?.Invoke(currencyType, amount);
            CurrencyChanged?.Invoke(currencyType);
        }

        public int GetAmount(CurrencyType currencyType) => GetCurrencySave(currencyType).Amount;

        private CurrencySave GetCurrencySave(CurrencyType currencyType) =>
            PlayerDataResourceData.GetOrCreateCurrencySave(currencyType);
    }
}
