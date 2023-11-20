using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.LocalizationService
{
    public class LocaleService : ILocaleService
    {
        private bool _currentLanguageHasBeenSet = false;
        private Dictionary<string, string> _currentLanguageStrings = new();

        private string _currentLanguage;
        private TextAsset _currentLocalizationText;

        private const string LocalizationKey = "locale";
        private const string LocalizationPrefix = "localization/";

        public bool CurrentLanguageHasBeenSet
        {
            get { return _currentLanguageHasBeenSet; }
            set { _currentLanguageHasBeenSet = value; }
        }
        public Dictionary<string, string> CurrentLanguageStrings => _currentLanguageStrings;
        public SystemLanguage SystemPlayerLanguage
        {
            get
            {
                return (SystemLanguage)PlayerPrefs.GetInt(LocalizationKey, (int)Application.systemLanguage);
            }
            set
            {
                PlayerPrefs.SetInt(LocalizationKey, (int)value);
                PlayerPrefs.Save();
            }
        }

        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    _currentLanguage = value;
                    _currentLocalizationText = Resources.Load(LocalizationPrefix + _currentLanguage, typeof(TextAsset)) as TextAsset;
                    if (_currentLocalizationText == null)
                    {
                        Debug.LogWarningFormat("Missing locale '{0}', loading English.", _currentLanguage);
                        _currentLanguage = SystemLanguage.English.ToString();
                        _currentLocalizationText = Resources.Load(LocalizationPrefix + _currentLanguage, typeof(TextAsset)) as TextAsset;
                    }
                    if (_currentLocalizationText != null)
                    {
                        string[] lines = _currentLocalizationText.text.Split(new string[] { "\r\n", "\n\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                        _currentLanguageStrings.Clear();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] pairs = lines[i].Split(new char[] { '\t', '=' }, 2);
                            if (pairs.Length == 2)
                            {
                                _currentLanguageStrings.Add(pairs[0].Trim(), pairs[1].Trim());
                            }
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("Local language '{0}', not found!", _currentLanguage);
                    }
                }
            }
        }
    }
}