using System;
using Sirenix.OdinInspector;

namespace Infrastructure.Services.Wallet
{
    [Serializable]
    public class Price
    {
        [HideLabel, HorizontalGroup] public CurrencyType CurrencyType;
        [HideLabel, HorizontalGroup] public int Amount;
    }
}