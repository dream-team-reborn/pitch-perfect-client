using System;
using System.Collections.Generic;
using com.trashpandaboy.core;
using PitchPerfect.DTO;

namespace PitchPerfect.Core
{
    public class MatchDataManager : Manager<MatchDataManager>
    {
        PhraseCardDTO _currentPhrase;
        public PhraseCardDTO CurrentPhrase => _currentPhrase;

        List<WordCardDTO> _currentHandOfCards;
        public List<WordCardDTO> CurrentHandOfCards => _currentHandOfCards;

        List<int> _selectedCards = new List<int>();
        public List<int> SelectedCards => _selectedCards;

        public Action OnCurrentPhraseUpdated = null;
        public Action OnCurrentHandOfCardsUpdated = null;
        public Action<int> OnCardSelected = null;
        public Action<int> OnCardUnselected = null;

        public void SetCurrentPhrase(PhraseCardDTO phraseDto)
        {
            _currentPhrase = phraseDto;

            OnCurrentPhraseUpdated?.Invoke();    
        }

        public void ReceivedCards(List<WordCardDTO> wordDto)
        {
            _currentHandOfCards.AddRange(wordDto);
            ResetSelectedCards();
            OnCurrentHandOfCardsUpdated?.Invoke();
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
            
        }

        public void UnselectCardInHand(int id)
        {
            if (_selectedCards == null)
                return;

            _selectedCards.Remove(id);
            OnCardUnselected?.Invoke(id);
        }

        public void ConfirmSelectedCards()
        {
            //TODO perform trigger of request with selected cards
        }
    }
}