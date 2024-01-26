using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PitchPerfect.UI
{
    public class UIWordCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cardText;
        [SerializeField] private Image _categoryImage;

        public void Setup(string cardText, int categoryId)
        {
            _cardText.text = cardText;
            _categoryImage.sprite = UIManager.Instance.SpritesDatabase.GetResource(categoryId);
        }
    }
}