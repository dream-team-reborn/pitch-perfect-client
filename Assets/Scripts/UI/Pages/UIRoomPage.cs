using PitchPerfect.DTO;
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

            PlayerDTO[] players = new PlayerDTO[3];

            players[0] = new PlayerDTO(0, "Paolo");
            players[1] = new PlayerDTO(1, "Mario");
            players[2] = new PlayerDTO(2, "Gino");

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
            
        }
    }
}