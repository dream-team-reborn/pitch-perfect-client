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
            
        }
        
        public void LeaveRoom()
        {
            
        }
    }
}