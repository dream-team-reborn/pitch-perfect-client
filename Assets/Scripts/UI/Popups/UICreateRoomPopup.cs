using PitchPerfect.Networking;
using TMPro;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UICreateRoomPopup : UIPopup
    {
        [SerializeField] private TMP_InputField _roomNameInput;

        public void OnCreateRoom()
        {
            if (string.IsNullOrEmpty(_roomNameInput.text))
                return;
            
            ServerManager.Instance.SendCreateRoomRequest(_roomNameInput.text);
            Hide();
        }

        public void OnClose()
        {
            Hide();
        }
    }
}