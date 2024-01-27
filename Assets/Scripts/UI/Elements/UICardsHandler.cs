using PitchPerfect.DTO;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UICardsHandler : MonoBehaviour
    {
        [SerializeField] private UIWordCard _cardPrefab;
        
        private UIWordCard[] _cards;

        public void PopulateCards(WordCardDTO[] wordCards)
        {
            _cards = new UIWordCard[wordCards.Length];
            
            for (int i = 0; i < wordCards.Length; i++)
            {
                _cards[i] = Instantiate(_cardPrefab, transform);
                _cards[i].Setup(wordCards[i].LocalizationKey, wordCards[i].CategoryId);
            }
        }
    }
}