using PitchPerfect.DTO;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIInGamePage : UIPage
    {
        [SerializeField] private UICardsHandler _cardsHandler;

        public override void Show()
        {
            base.Show();
            WordCardDTO[] wc = new WordCardDTO[3];
            for (int i = 0; i < wc.Length; i++)
            {
                wc[i] = new WordCardDTO(i, $"{i}_card", 0);
            }
            _cardsHandler.PopulateCards(wc);
        }
    }
}