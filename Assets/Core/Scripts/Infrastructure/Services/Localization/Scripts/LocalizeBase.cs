using UnityEngine;
using Zenject;

namespace Infrastructure.Services.LocalizationService
{
    public abstract class LocalizBase : MonoBehaviour
    {
        [SerializeField] protected string _localizationKey;

        public string LocalizationKey
        {
            get { return _localizationKey; }
            set { _localizationKey = value; }
        }

        protected ILocaleService _localeService;

        [Inject]
        public void Constructor(ILocaleService localeService)
        {
            _localeService = localeService;
        }

        public virtual void Initilize()
        {
            UpdateLocale();
        }

        public abstract void UpdateLocale();

        public string GetLocalizedString(string key)
        {
            if (_localeService.CurrentLanguageStrings.ContainsKey(key))
                return _localeService.CurrentLanguageStrings[key];
            else
                return string.Empty;
        }
    }
}
