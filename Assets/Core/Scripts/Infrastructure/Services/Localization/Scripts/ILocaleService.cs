using System.Collections.Generic;
using UnityEngine;

namespace LocalizationService
{
    public interface ILocaleService
    {
        public bool CurrentLanguageHasBeenSet { get; set; }
        public Dictionary<string, string> CurrentLanguageStrings { get; }
        public SystemLanguage SystemPlayerLanguage { get; set; }
        public string CurrentLanguage { get; set; }
    }
}