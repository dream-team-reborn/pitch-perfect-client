using System;
using System.Collections.Generic;
using com.trashpandaboy.core;
using Newtonsoft.Json;
using UnityEngine;


namespace Core
{
    public class LocalizationManager : Manager<LocalizationManager>
    {
        private const string DEFAULT_LANGUAGE = "en_EN";
        private const string MISSING_TRANSLATION = "<MISSING>";

        private Dictionary<string, string> loadedLocalizations;

        void Start()
        {
            loadedLocalizations = new Dictionary<string, string>();

            var jsonTextFile = Resources.Load<TextAsset>($"Localization/{DEFAULT_LANGUAGE}");
            if(jsonTextFile != null)
            {
                loadedLocalizations = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonTextFile.text);
                foreach(var kvp in loadedLocalizations)
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

            return MISSING_TRANSLATION;
        }
    }
}
