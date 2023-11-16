using UnityEngine;
using Zenject;

namespace Infrastructure.Services.LocalizationService
{
    public class LocalizeMenu : MonoBehaviour
    {
        [SerializeField] private Localize[] _localizes;

        private ILocaleService _localeService;

        [Inject]
        public void Constructor(ILocaleService localeService)
        {
            _localeService = localeService;
        }

        public void Initialize(Localize[] localizes)
        {
            _localizes = localizes;
        }

        public void SetEnglish() => ChangeLanguage(SystemLanguage.English);
        public void SetRussian() => ChangeLanguage(SystemLanguage.Russian);
        public void SetUkrainian() => ChangeLanguage(SystemLanguage.Ukrainian);

        private void ChangeLanguage(SystemLanguage systemLanguage)
        {
            _localeService.CurrentLanguage = systemLanguage.ToString();
            _localeService.SystemPlayerLanguage = systemLanguage;

            foreach (Localize localize in _localizes)
            {
                localize.UpdateLocale();
            }
        }
    }
}