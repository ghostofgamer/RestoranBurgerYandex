using System;
using Agava.YandexGames;
// using Lean.Localization;
using UnityEngine;

namespace LocalizationContent
{
    public class Localization : MonoBehaviour
    {
        /*private const string Language = "Language";
        private const string EnglishCode = "English";
        private const string RussianCode = "Russian";
        private const string TurkishCode = "Turkish";
        private const string English = "en";
        private const string Russian = "ru";
        private const string Turkish = "tr";

        // [SerializeField] private LeanLocalization _leanLocalization;

        private string _currentLanguage;
        private string _autoFoundLanguage;

        public event Action<string> LanguageChanged;
        
        public static Localization Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
#if UNITY_WEBGL && !UNITY_EDITOR
         /*_autoFoundLanguage = YandexGamesSdk.Environment.i18n.lang;
         _currentLanguage = _load.Get(Language, _autoFoundLanguage);
         SetLanguage(_currentLanguage);
         LanguageChanged?.Invoke(_currentLanguage);#1#

        /*_autoFoundLanguage = YandexGamesSdk.Environment.i18n.lang;
        SetLanguage(_currentLanguage);
        LanguageChanged?.Invoke(_currentLanguage);#1#
        
        
        _autoFoundLanguage = YandexGamesSdk.Environment.i18n.lang;
        SetLanguage(_autoFoundLanguage);
#endif
        }

        public void SetLanguage(string languageCode)
        {
            switch (languageCode)
            {
                case English:
                    _leanLocalization.SetCurrentLanguage(EnglishCode);  
                    break;

                case Turkish:
                    _leanLocalization.SetCurrentLanguage(TurkishCode);
                    break;

                case Russian:
                    _leanLocalization.SetCurrentLanguage(RussianCode);
                    break;
            }
        }
    
        public string GetCurrentLanguage()
        {
            Debug.Log(" language " + _autoFoundLanguage);
            Debug.Log(" language " + _autoFoundLanguage);
            Debug.Log(" language " + _autoFoundLanguage);
            return _autoFoundLanguage;
        }*/
    }
}