using System;
using System.Collections.Generic;
using com.trashpandaboy.core;
using Newtonsoft.Json;
using UnityEngine;


namespace PitchPerfect.Core
{
    public class LocalizationManager : Manager<LocalizationManager>
    {
        private List<string> _availablesLanguages = new List<string> { "it_IT", "en_EN" };
        private const string DEFAULT_LANGUAGE = "en_EN";
        private const string MISSING_TRANSLATION = "<MISSING>";

        private Dictionary<string, string> loadedLocalizations;

        private string _selectedLanguage = DEFAULT_LANGUAGE;

        public Action OnLocalizationChanged = null;

        protected override void Awake()
        {
            base.Awake();

            LoadLocalizations();
        }

        private void LoadLocalizations()
        {
            loadedLocalizations = new Dictionary<string, string>();

            var jsonTextFile = Resources.Load<TextAsset>($"Localization/{_selectedLanguage}");
            if (jsonTextFile != null)
            {
                loadedLocalizations = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonTextFile.text);
                foreach (var kvp in loadedLocalizations)
                {
                    Debug.Log($"Loaded localization entry - Key: {kvp.Key} - Value: {kvp.Value}");
                }
            }
        }

        public string GetLocalizedString(string key)
        {
            if(loadedLocalizations.ContainsKey(key))
            {
                return loadedLocalizations[key];
            }

            Debug.Log("Localization key not found: " + key);
            return MISSING_TRANSLATION;
        }

        public void SelectLanguage(string language)
        {
            if (language.Equals(_selectedLanguage))
                return;

            if (_availablesLanguages.Contains(language))
            {
                _selectedLanguage = language;
                LoadLocalizations();
                OnLocalizationChanged?.Invoke();
            }
        }

    }
}
