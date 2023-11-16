using UnityEngine;
using Zenject;

namespace Infrastructure.Services.LocalizationService
{
    public abstract class LocalizBase : MonoBehaviour
    {
        [SerializeField] protected string _localizationKey;

        protected ILocaleService _localeService;

        [Inject]
        public void Constructor(ILocaleService localeService)
        {
            _localeService = localeService;
        }

        public virtual void Initilize()
        {
            if (_localeService.CurrentLanguageHasBeenSet == false)
            {
                _localeService.CurrentLanguageHasBeenSet = true;
                SetCurrentLanguage(_localeService.SystemPlayerLanguage);
            }

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

        public void SetCurrentLanguage(SystemLanguage language)
        {
            _localeService.CurrentLanguage = language.ToString();
            _localeService.SystemPlayerLanguage = language;
            Localize[] allTexts = GameObject.FindObjectsOfType<Localize>();
            for (int i = 0; i < allTexts.Length; i++)
                allTexts[i].UpdateLocale();
        }
    }
}
