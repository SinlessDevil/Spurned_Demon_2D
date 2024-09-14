using System;

namespace Infrastructure.Services.Wallet
{
    public interface IWalletService
    {
        public event Action<CurrencyType> CurrencyChanged;
        public event Action<CurrencyType, int> CurrencyAdded;
        public event Action<CurrencyType, int> CurrencyRemoved;

        bool CanWithdraw(Price price);
        bool CanWithdraw(CurrencyType currencyType, int amount);
        void Withdraw(Price price);
        void Withdraw(CurrencyType currencyType, int amount);
        void Add(Price price);
        void Add(CurrencyType currencyType, int amount);
        int GetAmount(CurrencyType currencyType);
    }
}