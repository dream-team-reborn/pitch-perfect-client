using System;
using System.Linq;
using PitchPerfect.Core;
using PitchPerfect.DTO;
using PitchPerfect.Networking;
using TMPro;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIPhraseContainer : MonoBehaviour
    {
        private const string EMPTY_SPACE = "_______";

        [SerializeField] private GameObject _defaultBG;
        [SerializeField] private TMP_Text _phraseText;
        [Space] 
        [SerializeField] private GameObject _votedPlayer;
        [SerializeField] private TMP_Text _votedPlayerName;
        [Space]
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private TMP_Text _currentVoteNumber;
        [Space] 
        [SerializeField] private GameObject _leaderboardBG;
        [SerializeField] private UILeaderboardHandler _leaderboardHandler;

        private string _phrase;
        private int[] _wordsInPhrase;

        private void Start()
        {
            MatchDataManager.Instance.OnCardSelected += OnCardSelected;
            MatchDataManager.Instance.OnCardUnselected += OnCardUnselected;
            MatchDataManager.Instance.OnPlayerSelectionToVote += OnSelectionToVote;

            MatchDataManager.Instance.OnPlayerSelectedCardUpdated += SwitchToVote;
            ServerManager.Instance.OnAllUsersVoted += SwitchToLeaderboard;
            ServerManager.Instance.OnTurnStart += OnTurnStart;

            OnTurnStart();
        }

        private void OnDestroy()
        {
            MatchDataManager.Instance.OnCardSelected -= OnCardSelected;
            MatchDataManager.Instance.OnCardUnselected -= OnCardUnselected;
            MatchDataManager.Instance.OnPlayerSelectionToVote -= OnSelectionToVote;
            MatchDataManager.Instance.OnPlayerSelectedCardUpdated -= SwitchToVote;

            ServerManager.Instance.OnAllUsersVoted -= SwitchToLeaderboard;
            ServerManager.Instance.OnTurnStart -= OnTurnStart;
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

        private void OnTurnStart()
        {
            SwitchToCardSelection();
            PhraseCardDTO phraseCardDto = MatchDataManager.Instance.CurrentPhrase;
            Setup(phraseCardDto.GetLocalizedContent(), phraseCardDto.PlaceholderAmount);
        }

        private string GetCurrentFilledPhrase()
        {
            string phrase = _phrase;
            
            for (int i = 0; i < _wordsInPhrase.Length; i++)
            {
                if (_wordsInPhrase[i] >= 0)
                {
                    var wordDto = CardDataManager.Instance.GetWordCardById(_wordsInPhrase[i]);
                    var color = UIManager.Instance.ColorsDatabase.GetColor(wordDto.CategoryId);
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

        private void OnSelectionToVote()
        {
            var wordList = MatchDataManager.Instance.GetSelectionToVote();
            if (wordList != null)
            {
                Log.Info($"Selection to vote: {String.Join(",", wordList.Select(o => o.GetLocalizedContent()))}");

                _wordsInPhrase = wordList.Select(o => o.Id).ToArray();

                _phraseText.text = GetCurrentFilledPhrase();

                var votedPlayerId = MatchDataManager.Instance.GetUserIdOfSelectionToVote();
                _votedPlayerName.text = ServerManager.Instance.GetJoinedRoom().GetRoomPlayerById(votedPlayerId).Username;
            }
        }

        private void SwitchToCardSelection()
        {
            _timer.gameObject.SetActive(true);
            _currentVoteNumber.gameObject.SetActive(false);
            _votedPlayer.SetActive(false);
            
            _defaultBG.SetActive(true);
            _leaderboardBG.SetActive(false);
            _leaderboardHandler.gameObject.SetActive(false);
        }
        
        private void SwitchToVote()
        {
            _timer.gameObject.SetActive(false);
            _currentVoteNumber.gameObject.SetActive(true);
            _currentVoteNumber.text = "1";
            _votedPlayer.SetActive(true);
            
            _defaultBG.SetActive(true);
            _leaderboardBG.SetActive(false);
            _leaderboardHandler.gameObject.SetActive(false);
        }

        private void SwitchToLeaderboard()
        {
            _timer.gameObject.SetActive(true);
            _currentVoteNumber.gameObject.SetActive(false);
            _votedPlayer.SetActive(false);
            
            _defaultBG.SetActive(false);
            _leaderboardBG.SetActive(true);
            _leaderboardHandler.gameObject.SetActive(true);
            _leaderboardHandler.PopulateLeaderboard();
        }
    }
}