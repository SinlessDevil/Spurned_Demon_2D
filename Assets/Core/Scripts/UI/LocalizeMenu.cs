using Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.Services.LocalizationService
{
    public class LocalizeMenu : MonoBehaviour
    {
        [SerializeField] private Button _buttonRussian;
        [SerializeField] private Button _buttonEnglish;
        [SerializeField] private Button _buttonUkrainian;

        private bool _isInitialized = false;
        
        private Localize[] _localizes;

        private ILocaleService _localeService;

        [Inject]
        public void Constructor(ILocaleService localeService)
        {
            _localeService = localeService;
        }

        #region Init Methods
        public void Initialize(Localize[] localizes)
        {
            _localizes = localizes;

            Asserts();

            _isInitialized = true;
        }
        private void Asserts()
        {
            _buttonRussian.LogErrorIfComponentNull();
            _buttonEnglish.LogErrorIfComponentNull();
            _buttonUkrainian.LogErrorIfComponentNull();
        }
        #endregion

        #region Delegated Events Methods
        private void OnEnable()
        {
            if (_isInitialized)
                SubscribeEvents();
        }
        private void OnDisable()
        {
            if (_isInitialized)
                UnsubscribeEvents();
        }
        private void SubscribeEvents()
        {
            _buttonEnglish.onClick.AddListener(SetEnglish);
            _buttonRussian.onClick.AddListener(SetRussian);
            _buttonUkrainian.onClick.AddListener(SetUkrainian);
        }
        private void UnsubscribeEvents()
        {
            _buttonEnglish.onClick.RemoveListener(SetEnglish);
            _buttonRussian.onClick.RemoveListener(SetRussian);
            _buttonUkrainian.onClick.RemoveListener(SetUkrainian);
        }
        #endregion

        #region Handle Changed Language Methods
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
        #endregion
    }
}