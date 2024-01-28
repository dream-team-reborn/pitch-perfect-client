using System.Collections;
using com.trashpandaboy.core;
using PitchPerfect.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PitchPerfect.Core
{
    public class GameManager : Manager<GameManager>
    {
        //intuition
        private const float LOAD_MIN_DURATION = 0.2f;
        
        public void StartGame()
        {
            UIManager.Instance.Fader.Fade();
            StartCoroutine(LoadOffice());
        }

        public void EndGame()
        {
            UIManager.Instance.Fader.Fade();
            StartCoroutine(UnloadOffice());
        }

        private IEnumerator LoadOffice()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("TheOffice", LoadSceneMode.Additive);
            var startOperationT = Time.time;

            while (!asyncOperation.isDone || (Time.time - startOperationT) < LOAD_MIN_DURATION)
            {
                yield return 0;
            }
            
            UIManager.Instance.Show<UIInGamePage>();
            UIManager.Instance.Fader.Fade();
        }
        
        private IEnumerator UnloadOffice()
        {
            var asyncOperation = SceneManager.UnloadSceneAsync("TheOffice");
            
            var startOperationT = Time.time;

            while (!asyncOperation.isDone || (Time.time - startOperationT) < LOAD_MIN_DURATION)
            {
                yield return 0;
            }
            
            UIManager.Instance.Hide<UIInGamePage>();
            UIManager.Instance.Fader.Fade();
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