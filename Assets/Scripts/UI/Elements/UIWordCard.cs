using System;
using PitchPerfect.Core;
using PitchPerfect.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PitchPerfect.UI
{
    public class UIWordCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cardText;
        [SerializeField] private Image _categoryImage;
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
            _categoryImage.sprite = UIManager.Instance.SpritesDatabase.GetResource(categoryId);
        }

        public void HighlightCard()
        {
            _tween.Play();
        }

        public void LowlightCard()
        {
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
        }
        
        private void OnCardUnselected(int id)
        {
            if (id != _cardId)
                return;

            _isSelected = false;
        }
    }
}