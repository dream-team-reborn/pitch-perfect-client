using System;
using System.Collections.Generic;
using PitchPerfect.Core;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PitchPerfect.UI
{
    public class UIPhraseContainer : MonoBehaviour
    {
        private const string EMPTY_SPACE = "_______";
        
        [SerializeField] private TMP_Text _phraseText;

        private string _phrase;
        private int[] _wordsInPhrase;

        private void Start()
        {
            MatchDataManager.Instance.OnCardSelected += OnCardSelected;
            MatchDataManager.Instance.OnCardUnselected += OnCardUnselected;

            int rnd = Random.Range(1, 21);
            var phraseCardDto = CardDataManager.Instance.GetPhraseCardById(rnd);
            
            MatchDataManager.Instance.SetCurrentPhrase(phraseCardDto);
            
            Setup(phraseCardDto.GetLocalizedContent(), phraseCardDto.PlaceholderAmount);
        }

        private void OnDestroy()
        {
            MatchDataManager.Instance.OnCardSelected -= OnCardSelected;
            MatchDataManager.Instance.OnCardUnselected += OnCardUnselected;
        }

        private void Setup(string phrase, int placeholdersAmount)
        {
            _wordsInPhrase = new int[placeholdersAmount];
            
            _phrase = phrase;
            for (int i = 0; i < placeholdersAmount; i++)
            {
                _wordsInPhrase[i] = -1;
                phrase = phrase.Replace($"${i + 1}", EMPTY_SPACE);
            }

            _phraseText.text = phrase;
        }

        private string GetCurrentFilledPhrase()
        {
            string phrase = _phrase;
            
            for (int i = 0; i < _wordsInPhrase.Length; i++)
            {
                if (_wordsInPhrase[i] >= 0)
                {
                    var wordDto = CardDataManager.Instance.GetWordCardById(_wordsInPhrase[i]);
                    var color = Color.red;
                    phrase = phrase.Replace($"${i + 1}", $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{wordDto.GetLocalizedContent()}</color>");
                }
                else
                {
                    phrase = phrase.Replace($"${i + 1}", EMPTY_SPACE);
                }
            }

            return phrase;
        }
        
        private void OnCardSelected(int id)
        {
            for (int i = 0; i < _wordsInPhrase.Length; i++)
            {
                if (_wordsInPhrase[i] < 0)
                {
                    _wordsInPhrase[i] = id;
                    break;
                }
            }
            
            _phraseText.text = GetCurrentFilledPhrase();
        }
        
        private void OnCardUnselected(int id)
        {
            for (int i = 0; i < _wordsInPhrase.Length; i++)
            {
                if (_wordsInPhrase[i] != id)
                    continue;

                _wordsInPhrase[i] = -1;
                _phraseText.text = GetCurrentFilledPhrase().Replace($"${i + 1}", EMPTY_SPACE);
                return;
            }
        }
    }
}