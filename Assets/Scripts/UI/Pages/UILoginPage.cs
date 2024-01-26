using PitchPerfect.Networking;
using TMPro;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UILoginPage : UIPage
    {
        [SerializeField] private LoginHandler _loginHandler;
        [SerializeField] private TMP_InputField _usernameInput;

        public void OnLoginButtonClick()
        {
            _loginHandler.RequestLogin(_usernameInput.text);
        }
    }
}