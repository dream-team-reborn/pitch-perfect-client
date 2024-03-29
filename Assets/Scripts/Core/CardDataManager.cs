using System;
using System.Collections.Generic;
using System.Linq;
using com.trashpandaboy.core;
using Newtonsoft.Json;
using PitchPerfect.DTO;
using UnityEngine;

namespace PitchPerfect.Core
{
    public class CardDataManager : Manager<CardDataManager>
    {
        public static readonly int CARDS_IN_HAND = 4;
        
        Dictionary<int, PhraseCardDTO> _phraseCardsWholeList;
        Dictionary<int, WordCardDTO> _wordCardsWholeList;
        Dictionary<int, WordCardCategoryDTO> _cardCategoriesWholeList;
        
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
                Log.Info($"Category Id: {category.id} - '{_cardCategoriesWholeList[category.id].GetLocalizedContent()}'");
            }

            foreach (var word in config.words)
            {
                _wordCardsWholeList[word.id] = word.ConvertToDTO(config.localization_keys[1]);
                Log.Info($"Word Id: {word.id} - '{_wordCardsWholeList[word.id].GetLocalizedContent()}' of Category: {_wordCardsWholeList[word.id].Category.GetLocalizedContent()}");
            }

            foreach (var phrase in config.phrases)
            {
                _phraseCardsWholeList[phrase.id] = phrase.ConvertToDTO(config.localization_keys[0]);
                Log.Info($"Phrase Id: {phrase.id} - '{_phraseCardsWholeList[phrase.id].GetLocalizedContent()}'");
            }
        }

        private void ClearDataDictionaries()
        {
            _phraseCardsWholeList = new Dictionary<int, PhraseCardDTO>();
            _wordCardsWholeList = new Dictionary<int, WordCardDTO>();
            _cardCategoriesWholeList = new Dictionary<int, WordCardCategoryDTO>();
        }

        public PhraseCardDTO GetPhraseCardById(int id)
        {
            return _phraseCardsWholeList[id];
        }

        public WordCardDTO GetWordCardById(int id)
        {
            return _wordCardsWholeList[id];
        }

        public List<WordCardDTO> GetWordCardListByIds(List<int> ids)
        {
            return _wordCardsWholeList.Where(o => ids.Contains(o.Key)).Select(o => o.Value).ToList();
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
