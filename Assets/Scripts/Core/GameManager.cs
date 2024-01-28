using com.trashpandaboy.core;
using PitchPerfect.UI;
using UnityEngine.SceneManagement;

namespace PitchPerfect.Core
{
    public class GameManager : Manager<GameManager>
    {
        public void StartGame()
        {
            UIManager.Instance.Show<UIInGamePage>();
            SceneManager.LoadScene("TheOffice", LoadSceneMode.Additive);
        }

        public void EndGame()
        {
            UIManager.Instance.Hide<UIInGamePage>();
            SceneManager.UnloadSceneAsync("TheOffice");
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