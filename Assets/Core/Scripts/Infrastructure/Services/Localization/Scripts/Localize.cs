using System;
using UnityEngine;
using Extensions;
using TMPro;

namespace LocalizationService
{
    [RequireComponent(typeof(TMP_Text))]
    public class Localize : LocalizBase
    {
        private TMP_Text _text;

        public override void Initilize()
        {
            InitComponent();
            Asserts();
            base.Initilize();
        }

        private void InitComponent() => _text = GetComponent<TMP_Text>();
        private void Asserts() => _text.LogErrorIfComponentNull();

        public override void UpdateLocale()
        {
            if (_text == false) 
                return;

            if (String.IsNullOrEmpty(_localizationKey) && _localeService.CurrentLanguageStrings.ContainsKey(_localizationKey) == false)
                _text.text = _localeService.CurrentLanguageStrings[_localizationKey].Replace(@"\n", "" + '\n'); ;
        }
    }
}
