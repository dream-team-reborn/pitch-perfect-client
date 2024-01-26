using PitchPerfect.UI;
using UnityEngine;

namespace PitchPerfect.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        private void Start()
        {
            UIManager.Instance.Show<UILoginPage>();
        }

        public void RequestLogin(string username)
        {
            //TODO: Send HTTP message with username
            Debug.Log($"Requesting Login with username {username}");
        }
    }
}