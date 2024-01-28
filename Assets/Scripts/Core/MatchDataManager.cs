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

        public Action OnCurrentPhraseUpdated = null;
        public Action OnCurrentHandOfCardsUpdated = null;
        public Action<int> OnCardSelected = null;
        public Action<int> OnCardUnselected = null;
        public Action OnPlayerListUpdated = null;
        public Action OnTrendsUpdated = null;
        public Action OnPlayerSelectedCardUpdated = null;

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
            _playersSelectedCards = new Dictionary<string, List<WordCardDTO>>();
            foreach(var kvp in selectedCards)
            {
                _playersSelectedCards[kvp.Key] = CardDataManager.Instance.GetWordCardListByIds(kvp.Value);
            }
            OnPlayerSelectedCardUpdated?.Invoke();
        }
    }
}
