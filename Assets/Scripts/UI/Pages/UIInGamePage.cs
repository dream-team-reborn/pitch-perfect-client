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
            while(randomIds.Count < 4)
            {
                int rnd = Random.Range(1, 60);
                if (!randomIds.Contains(rnd))
                    randomIds.Add(rnd);
            }

            WordCardDTO[] wc = new WordCardDTO[4];
            for (int i = 0; i < wc.Length; i++)
            {
                wc[i] = CardDataManager.Instance.GetWordCardById(randomIds[i]);
            }
            _cardsHandler.PopulateCards(wc);
        }
    }
}