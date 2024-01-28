using System.Collections.Generic;
using PitchPerfect.Core;
using PitchPerfect.DTO;
using PitchPerfect.Networking;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIInGamePage : UIPage
    {
        [SerializeField] private UICardsHandler _cardsHandler;

        private void Awake()
        {
            ServerManager.Instance.OnTurnStart += OnTurnStart;

            OnTurnStart();
        }
        private void OnDestroy()
        {
            ServerManager.Instance.OnTurnStart -= OnTurnStart;
        }

        private void OnTurnStart()
        {
            _cardsHandler.PopulateCards(MatchDataManager.Instance.CurrentHandOfCards.ToArray());
        }
    }
}