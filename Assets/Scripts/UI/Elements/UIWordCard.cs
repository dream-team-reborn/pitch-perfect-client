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

        public void Setup(string cardText, int categoryId)
        {
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
    }
}