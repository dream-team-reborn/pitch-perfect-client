using System;
using PitchPerfect.Core;
using PitchPerfect.DTO;
using PitchPerfect.Networking;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIRoomPage : UIPage
    {
        [SerializeField] private UIPlayer _playerPrefab;
        [SerializeField] private Transform _playersContainer;
        
        private UIPlayer[] _players;

        private void Start()
        {
            MatchDataManager.Instance.OnPlayerListUpdated += ReloadPlayersList;
        }

        private void OnDestroy()
        {
            MatchDataManager.Instance.OnPlayerListUpdated -= ReloadPlayersList;
        }

        public override void Show()
        {
            base.Show();
            ReloadPlayersList();
        }

        private void ReloadPlayersList()
        {
            PlayerDTO[] players = new PlayerDTO[0];
            RoomDTO roomJoined = ServerManager.Instance.GetJoinedRoom();
            if(roomJoined != null)
            {
                if(roomJoined.Players != null)
                {
                    players = roomJoined.Players.ToArray();
                }
            }
            PopulatePlayersList(players);
        }
        
        public void PopulatePlayersList(PlayerDTO[] players)
        {
            if (_players != null)
            {
                foreach (var room in _players)
                {
                    Destroy(room.gameObject);
                }
            }
            
            _players = new UIPlayer[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                var uiPlayer = Instantiate(_playerPrefab, _playersContainer);
                uiPlayer.Setup(players[i].Id, players[i].Username);
                _players[i] = uiPlayer;
            }
        }

        public void OnReady()
        {
            ServerManager.Instance.SendPlayerReady();
        }
    }
}