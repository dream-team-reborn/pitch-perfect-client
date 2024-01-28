using PitchPerfect.Core;
using UnityEngine;

namespace PitchPerfect.GameDebug
{
    public class DebugHandler : MonoBehaviour
    {
        public void OnStartGameClick()
        {
            GameManager.Instance.StartGame();
        }
        
        public void OnEndGameClick()
        {
            GameManager.Instance.EndGame();
        }
    }
}