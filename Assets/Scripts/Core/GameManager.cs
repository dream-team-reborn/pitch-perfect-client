using com.trashpandaboy.core;
using PitchPerfect.UI;

namespace PitchPerfect.Core
{
    public class GameManager : Manager<GameManager>
    {
        public void StartGame()
        {
            UIManager.Instance.Show<UIInGamePage>();
        }

        public void EndGame()
        {
            UIManager.Instance.Hide<UIInGamePage>();
        }

        public void JoinRoom()
        {
            UIManager.Instance.Show<UIRoomPage>();
        }
        
        public void LeaveRoom()
        {
            
        }

        public void OnRoomsListJoined()
        {
            UIManager.Instance.Show<UIRoomsListPage>();
        }
    }
}