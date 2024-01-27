using System.Collections;
using com.trashpandaboy.core;
using Newtonsoft.Json;
using PitchPerfect.Login.DTO;
using PitchPerfect.Networking.Messages;
using PitchPerfect.UI;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;

namespace PitchPerfect.Networking
{
    public class ServerManager : Manager<ServerManager>
    {
        static string POST_LOGIN_ENDPOINT = "http://[2a03:b0c0:3:d0::76d:d001]:8080/login";
        static string WEBSOCKET_ENDPOINT = "ws://[2a03:b0c0:3:d0::76d:d001]:8080/ws?token=$";

        WebSocket _webSocket = null;
        AuthorizedUserDTO _authorizedUser = null;

        private void Start()
        {
            //UIManager.Instance.Show<UILoginPage>();

        }

        public void RequestLogin(string username)
        {
            //TODO: Send HTTP message with username
            Debug.Log($"Requesting Login with username {username}");

            StartCoroutine(SendLoginRequest(username));
        }

        IEnumerator SendLoginRequest(string username)
        {
            Debug.Log("Retrieving authorization token...");
            string jsonContent = JsonConvert.SerializeObject(new LoginDTO(username, "").ConvertToJson());

            using (UnityWebRequest postRequest = UnityWebRequest.Post(POST_LOGIN_ENDPOINT, jsonContent, "application/json"))
            {
                yield return postRequest.SendWebRequest();

                if (postRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(postRequest.error);
                }
                else
                {
                    _authorizedUser = JsonConvert.DeserializeObject<AuthorizedUserDTO>(postRequest.downloadHandler.text);
                    Debug.Log($"Retrieved authorized user \n {_authorizedUser}");
                    OpenWebSocket();
                }
            }
        }

        private void OpenWebSocket()
        {
            string endPoint = WEBSOCKET_ENDPOINT.Replace("$", _authorizedUser.Token);
            Debug.Log("Called OpenWebSocket - EndPoint: " + endPoint);
            _webSocket = new WebSocket(endPoint);
            _webSocket.Connect();
            _webSocket.OnMessage += (sender, e) =>
            {
                Debug.Log($"[Server '{((WebSocket)sender).Url}']: {e.Data}");
            };

        }

        private void Update()
        {
            if (_webSocket == null)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _webSocket.Send("Test Echo");
            }
        }

        #region Requests

        public void SendListRoom()
        {

        }

        public void SendJoinRoom(string roomId)
        {

        }

        public void SendLeaveRoom()
        {

        }

        public void SendPlayerReady()
        {

        }

        public void SendCardSelection()
        {

        }

        public void SendCombinationChoice()
        {

        }

        #endregion

        #region Responses

        private void HandleRoomListReceived()
        {

        }

        private void HandleRoomJoined()
        {

        }

        private void HandleRoomLeft()
        {

        }

        private void HandleUserJoined()
        {

        }

        private void HandlePlayerReady()
        {

        }

        private void HandleMatchStarted()
        {

        }

        private void HandleTurnStarted()
        {

        }

        private void HandlePlayerCombinationChoices()
        {

        }

        private void HandleTurnEnd()
        {

        }


        #endregion
    }
}