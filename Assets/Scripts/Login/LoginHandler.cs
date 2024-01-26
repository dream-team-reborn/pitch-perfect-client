using UnityEngine;

namespace PitchPerfect.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        public void RequestLogin(string username)
        {
            //TODO: Send HTTP message with username
            Debug.Log($"Requesting Login with username {username}");
        }
    }
}