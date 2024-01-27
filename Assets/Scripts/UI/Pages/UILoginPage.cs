using PitchPerfect.Networking;
using TMPro;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UILoginPage : UIPage
    {
        [SerializeField] private TMP_InputField _usernameInput;

        public void OnLoginButtonClick()
        {
            ServerManager.Instance.RequestLogin(_usernameInput.text);
        }
    }
}