using System.Collections;
using com.trashpandaboy.core;
using Newtonsoft.Json;
using PitchPerfect.Login.DTO;
using PitchPerfect.UI;
using PitchPerfect.Core;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using System.Net.WebSockets;
using System;
using System.Threading;
using System.IO;
using PitchPerfect.Networking.Messages;

namespace PitchPerfect.Networking
{
    public class ServerManager : Manager<ServerManager>
    {
        static string POST_LOGIN_ENDPOINT = "http://[2a03:b0c0:3:d0::76d:d001]:8080/login";

        static string WEBSOCKET_ENDPOINT = "ws://[2a03:b0c0:3:d0::76d:d001]:8080/ws?token=$";

        private SocketHandler _socketHandler;

        ClientWebSocket _webSocket = null;

        public ClientWebSocket WebSocket => _webSocket;

        AuthorizedUserDTO _authorizedUser = null;

        private void Start()
        {
            UIManager.Instance.Show<UILoginPage>();
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
                    StartCoroutine(OpenClientSocket());
                }
            }
        }

        IEnumerator OpenClientSocket()
        {
            _socketHandler = new SocketHandler(WEBSOCKET_ENDPOINT.Replace("$",_authorizedUser.Token));
            ConnectToServer();

            yield break;

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


        /// <summary>
        /// Unity method called every frame
        /// </summary>
        private void Update()
        {
            if (_socketHandler == null)
                return;

            // Check if server send new messages
            var cqueue = _socketHandler.receiveQueue;
            string msg;
            while (cqueue.TryPeek(out msg))
            {
                // Parse newly received messages
                cqueue.TryDequeue(out msg);
                HandleMessage(msg);
            }
        }
        /// <summary>
        /// Method responsible for handling server messages
        /// </summary>
        /// <param name="msg">Message.</param>
        private void HandleMessage(string msg)
        {
            Debug.Log("Server: " + msg);
        }
        /// <summary>
        /// Call this method to connect to the server
        /// </summary>
        public async void ConnectToServer()
        {
            await _socketHandler.Connect();
        }
        /// <summary>
        /// Method which sends data through websocket
        /// </summary>
        /// <param name="message">Message.</param>
        public void SendRequest(string message)
        {
            _socketHandler.Send(message);
        }

    }
}