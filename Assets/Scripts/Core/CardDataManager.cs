using System.Collections.Generic;
using com.trashpandaboy.core;
using Newtonsoft.Json;
using PitchPerfect.DTO;
using UnityEngine;

namespace PitchPerfect.Core
{
    public class CardDataManager : Manager<CardDataManager>
    {
        Dictionary<int, PhraseCardDTO> _phraseCardsWholeList;
        Dictionary<int, WordCardDTO> _wordCardsWholeList;
        Dictionary<int, WordCardCategoryDTO> _cardCategoriesWholeList;

        protected override void Awake()
        {
            base.Awake();

            //THIS IS TEMPORARY UNITL SERVER SEND US THE CONFIG
            var tempTextConfig = Resources.Load<TextAsset>("game_configuration");

            OnConfigReceived(tempTextConfig.text);
        }

        public void OnConfigReceived(string configJson)
        {
            GameConfig config = JsonConvert.DeserializeObject<GameConfig>(configJson);

            FillDictionariesWithConfig(config);
        }

        private void FillDictionariesWithConfig(GameConfig config)
        {
            ClearDataDictionaries();

            foreach(var category in config.categories)
            {
                _cardCategoriesWholeList[category.id] = category.ConvertToDTO(config.localization_keys[2]);
                Debug.Log($"Category Id: {category.id} - '{_cardCategoriesWholeList[category.id].GetLocalizedContent()}'");
            }

            foreach (var word in config.words)
            {
                _wordCardsWholeList[word.id] = word.ConvertToDTO(config.localization_keys[1]);
                Debug.Log($"Word Id: {word.id} - '{_wordCardsWholeList[word.id].GetLocalizedContent()}' of Category: {_wordCardsWholeList[word.id].Category.GetLocalizedContent()}");
            }

            foreach (var phrase in config.phrases)
            {
                _phraseCardsWholeList[phrase.id] = phrase.ConvertToDTO(config.localization_keys[0]);
                Debug.Log($"Phrase Id: {phrase.id} - '{_phraseCardsWholeList[phrase.id].GetLocalizedContent()}'");
            }
        }

        private void ClearDataDictionaries()
        {
            if (_phraseCardsWholeList == null)
                _phraseCardsWholeList = new Dictionary<int, PhraseCardDTO>();
            else
                _phraseCardsWholeList.Clear();

            if (_wordCardsWholeList == null)
                _wordCardsWholeList = new Dictionary<int, WordCardDTO>();
            else
                _wordCardsWholeList.Clear();

            if (_cardCategoriesWholeList == null)
                _cardCategoriesWholeList = new Dictionary<int, WordCardCategoryDTO>();
            else
                _cardCategoriesWholeList.Clear();
        }

        public PhraseCardDTO GetPhraseCardById(int id)
        {
            return _phraseCardsWholeList[id];
        }

        public WordCardDTO GetWordCardById(int id)
        {
            return _wordCardsWholeList[id];
        }

        public WordCardCategoryDTO GetCardCategoryById(int id)
        {
            return _cardCategoriesWholeList[id];
        }

        public class GameConfig
        {
            public List<string> localization_keys;
            public List<PhraseEntry> phrases;
            public List<WordEntry> words;
            public List<CategoryEntry> categories;

            public class PhraseEntry
            {
                public int id;
                public int placeholdersAmount;

                public PhraseCardDTO ConvertToDTO(string baseLocalize)
                {
                    return new PhraseCardDTO(id, baseLocalize.Replace("$", $"{id}".PadLeft(3, '0')), placeholdersAmount);
                }
            }

            public class WordEntry
            {
                public int id;
                public int categoryId;

                public WordCardDTO ConvertToDTO(string baseLocalize)
                {
                    return new WordCardDTO(id, baseLocalize.Replace("$", $"{id}".PadLeft(3, '0')), categoryId);
                }
            }

            public class CategoryEntry
            {
                public int id;

                public WordCardCategoryDTO ConvertToDTO(string baseLocalize)
                {
                    return new WordCardCategoryDTO(id, baseLocalize.Replace("$", $"{id}".PadLeft(3, '0')));
                }
            }
        } 
    }
}
