using PitchPerfect.Core;
using PitchPerfect.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PitchPerfect.UI
{
    public class UIWordCard : MonoBehaviour
    {
        private const string CARD_SPRITE_PREFIX = "card_";
        
        [SerializeField] private TMP_Text _cardText;
        [SerializeField] private Image _background;
        [SerializeField] private PositionTween _tween;

        private int _cardId;
        private bool _isSelected;

        private void Start()
        {
            MatchDataManager.Instance.OnCardSelected += OnCardSelected;
            MatchDataManager.Instance.OnCardUnselected += OnCardUnselected;
        }

        private void OnDestroy()
        {
            MatchDataManager.Instance.OnCardSelected -= OnCardSelected;
            MatchDataManager.Instance.OnCardUnselected -= OnCardUnselected;
        }

        public void Setup(int cardId, string cardText, int categoryId)
        {
            _cardId = cardId;
            _cardText.text = cardText;
            _background.sprite = UIManager.Instance.SpritesDatabase.GetResource(CARD_SPRITE_PREFIX + categoryId);
        }

        public void HighlightCard()
        {
            if (_isSelected)
                return;
            
            _tween.Play();
        }

        public void LowlightCard()
        {
            if (_isSelected)
                return;
            
            _tween.Play(true, true);
        }

        public void OnCardClick()
        {
            if (_isSelected)
            {
                MatchDataManager.Instance.UnselectCardInHand(_cardId);
            }
            else
            {
                MatchDataManager.Instance.SelectCardInHand(_cardId);
            }
        }
        
        private void OnCardSelected(int id)
        {
            if (id != _cardId)
                return;

            _isSelected = true;
            _tween.SnapToEnd();
        }
        
        private void OnCardUnselected(int id)
        {
            if (id != _cardId)
                return;

            _isSelected = false;
            _tween.SnapToStart();
        }
    }
}