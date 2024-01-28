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
        
        public override void Show()
        {
            base.Show();

            PopulatePlayersList(ServerManager.Instance.GetJoinedRoom().Players.ToArray());
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
            
        }
    }
}