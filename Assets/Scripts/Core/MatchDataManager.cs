using System;
using System.Collections.Generic;
using System.Linq;
using com.trashpandaboy.core;
using PitchPerfect.DTO;
using PitchPerfect.Networking;

namespace PitchPerfect.Core
{
    public class MatchDataManager : Manager<MatchDataManager>
    {
        List<PlayerDTO> _matchPlayers;
        public List<PlayerDTO> MatchPlayers => _matchPlayers;

        PhraseCardDTO _currentPhrase;
        public PhraseCardDTO CurrentPhrase => _currentPhrase;

        List<WordCardDTO> _currentHandOfCards;
        public List<WordCardDTO> CurrentHandOfCards => _currentHandOfCards;

        Dictionary<int, int> _categoryTrends;
        public Dictionary<int, int> CategoryTrends => _categoryTrends;

        List<int> _selectedCards = new List<int>();
        public List<int> SelectedCards => _selectedCards;

        Dictionary<string, List<WordCardDTO>> _playersSelectedCards;
        public Dictionary<string, List<WordCardDTO>> PlayersSelectedCards => _playersSelectedCards;

        List<bool> _selectionVotes = new List<bool>();

        public Action OnCurrentPhraseUpdated = null;
        public Action OnCurrentHandOfCardsUpdated = null;
        public Action<int> OnCardSelected = null;
        public Action<int> OnCardUnselected = null;
        public Action OnPlayerListUpdated = null;
        public Action OnTrendsUpdated = null;
        public Action OnPlayerSelectedCardUpdated = null;
        public Action OnPlayerSelectionToVote = null;

        int _currentSelectionToVote = 0;

        public void SetCurrentPhrase(PhraseCardDTO phraseDto)
        {
            _currentPhrase = phraseDto;

            OnCurrentPhraseUpdated?.Invoke();
        }

        public void ReceivedCards(List<WordCardDTO> wordDto)
        {
            _currentHandOfCards = wordDto;
            ResetSelectedCards();
            OnCurrentHandOfCardsUpdated?.Invoke();
        }

        public void ReceivedPlayer(PlayerDTO player)
        {
            if (_matchPlayers == null)
                _matchPlayers = new List<PlayerDTO>();

            _matchPlayers.Add(player);
            OnPlayerListUpdated?.Invoke();
        }

        public void ReceivedTrends(Dictionary<string, int> trends)
        {
            _categoryTrends = new Dictionary<int, int>();
            foreach (var kvp in trends)
            {
                _categoryTrends[int.Parse(kvp.Key)] = kvp.Value;
            }
            OnTrendsUpdated?.Invoke();
        }

        private void ResetSelectedCards()
        {
            _selectedCards?.Clear();
        }

        public void SelectCardInHand(int id)
        {
            if (_selectedCards == null)
                _selectedCards = new List<int>();
            
            _selectedCards.Add(id);
            OnCardSelected?.Invoke(id);

            if(_selectedCards.Count == _currentPhrase.PlaceholderAmount)
            {
                ServerManager.Instance.SendCardSelection();
            }
        }

        public void UnselectCardInHand(int id)
        {
            if (_selectedCards == null)
                return;

            _selectedCards.Remove(id);
            OnCardUnselected?.Invoke(id);
        }


        internal void ReceivedPlayersSelection(List<KeyValuePair<string, List<int>>> selectedCards)
        {
            _currentSelectionToVote = 0;
            _selectionVotes = new List<bool>();
            _playersSelectedCards = new Dictionary<string, List<WordCardDTO>>();

            foreach(var kvp in selectedCards)
            {
                _playersSelectedCards[kvp.Key] = new List<WordCardDTO>();
                    
                foreach(var wordId in kvp.Value)
                {
                    _playersSelectedCards[kvp.Key].Add(CardDataManager.Instance.GetWordCardById(wordId));
                }
            }

            OnPlayerSelectedCardUpdated?.Invoke();
            FirstSelectionToVote();
        }

        public List<WordCardDTO> GetSelectionToVote()
        {
            string userId = _playersSelectedCards.Keys.ElementAt(_currentSelectionToVote);
            return _playersSelectedCards[userId];
        }
        public void VoteCurrentSelection(bool vote)
        {
            _selectionVotes.Add(vote);
            NextSelectionToVote();
        }

        public void FirstSelectionToVote()
        {
            OnPlayerSelectionToVote?.Invoke();
        }

        public void NextSelectionToVote()
        {
            if (_currentSelectionToVote < _playersSelectedCards.Keys.Count - 1)
            {
                _currentSelectionToVote++;
                OnPlayerSelectionToVote?.Invoke();
            }
            else
            {
                Dictionary<string, bool> _votesPerUser = new Dictionary<string, bool>();

                int voteIndex = 0;

                foreach(var kvp in PlayersSelectedCards)
                {
                    _votesPerUser[kvp.Key] = _selectionVotes.ElementAt(voteIndex);
                    voteIndex++;
                }
                ServerManager.Instance.SendVoteOfSelection(_votesPerUser);
            }
        }

    }
}
