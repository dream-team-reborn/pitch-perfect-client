using System.Collections.Generic;
using PitchPerfect.Core;
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
            List<int> randomIds = new List<int>();
            while(randomIds.Count < CardDataManager.CARDS_IN_HAND)
            {
                int rnd = Random.Range(1, 61);
                if (!randomIds.Contains(rnd))
                    randomIds.Add(rnd);
            }

            WordCardDTO[] wc = new WordCardDTO[CardDataManager.CARDS_IN_HAND];
            for (int i = 0; i < wc.Length; i++)
            {
                wc[i] = CardDataManager.Instance.GetWordCardById(randomIds[i]);
            }
            _cardsHandler.PopulateCards(wc);
        }
    }
}