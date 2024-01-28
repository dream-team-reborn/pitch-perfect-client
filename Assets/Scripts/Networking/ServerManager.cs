using System.Collections;
using com.trashpandaboy.core;
using Newtonsoft.Json;
using PitchPerfect.Login.DTO;
using PitchPerfect.UI;
using PitchPerfect.Core;
using UnityEngine;
using UnityEngine.Networking;
using System;
using PitchPerfect.Networking.Messages;
using PitchPerfect.Networking.Responses;
using PitchPerfect.DTO;
using System.Collections.Generic;
using System.Linq;

namespace PitchPerfect.Networking
{
    public class ServerManager : Manager<ServerManager>
    {
        static string POST_LOGIN_ENDPOINT = "http://[2a03:b0c0:3:d0::76d:d001]:8080/login";

        static string WEBSOCKET_ENDPOINT = "ws://[2a03:b0c0:3:d0::76d:d001]:8080/ws?token=$";
        //static string WEBSOCKET_ENDPOINT = "ws://192.168.1.201:8080/ws?token=$";

        private SocketHandler _socketHandler;

        AuthorizedUserDTO _authorizedUser = null;

        List<RoomDTO> _rooms = new List<RoomDTO>();
        RoomDTO _joinedRoom = null;

        private Action OnGameStart = null;
        private Action OnTurnStart = null;
        private Action OnAllUsersSelectedCards = null;
        private Action OnAllUsersVoted = null;
        private Action OnTurnEnd = null;
        private Action OnGameEnd = null;

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
            Debug.Log("Retrieving authorization token... endpoint: " + POST_LOGIN_ENDPOINT);
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
            StartCoroutine(RetrieveRoomList());
            yield break;
        }

        IEnumerator RetrieveRoomList()
        {
            Debug.Log("RetrieveRoomList");
            if (!_socketHandler.IsConnectionOpen())
            {
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(RetrieveRoomList());
                yield break;
            }
            
            Debug.Log("RetrieveRoomList - IsConnectionOpen: " + _socketHandler.IsConnectionOpen());
            SendListRoomRequest();
        }

        #region Requests

        public void SendListRoomRequest()
        {
            string message = new GetRoomsMessage().ConvertToJson();
            Debug.Log("Sending message: " + message);
            _socketHandler.Send(message);
        }

        public void SendJoinRoom(string roomId)
        {
            string message = new JoinRoomMessage(roomId).ConvertToJson();
            Debug.Log("Sending message: " + message);
            _joinedRoom = _rooms.Where(o => o.Id.Equals(roomId)).Single();
            _socketHandler.Send(message);
        }

        public void SendLeaveRoom()
        {
            string message = new LeaveRoomMessage(_joinedRoom.Id).ConvertToJson();
            Debug.Log("Sending message: " + message);
            _socketHandler.Send(message);
        }

        public void SendPlayerReady()
        {
            string message = new PlayerReadyMessage(_joinedRoom.Id).ConvertToJson();
            Debug.Log("Sending message: " + message);
            _socketHandler.Send(message);
        }

        public void SendCardSelection()
        {

        }

        public void SendCombinationChoice()
        {

        }

        #endregion

        #region Responses

        private void HandleRoomListReceived(string msg)
        {
            GetRoomsResponse response = JsonConvert.DeserializeObject<GetRoomsResponse>(msg);

            _rooms = response.Rooms.Select(o => o.ConvertToDTO()).ToList();

            GameManager.Instance.OnRoomsListJoined();

        }

        private void HandleRoomJoined(string msg)
        {
            JoinRoomResponse response = JsonConvert.DeserializeObject<JoinRoomResponse>(msg);
            Debug.Log($"HandleRoomJoined - Result: {response.Result}");

            if (response.Result)
            {
                GameManager.Instance.JoinRoom();
            }

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

        private void HandleMatchStarted(string msg)
        {
            GameStartedResponse response = JsonConvert.DeserializeObject<GameStartedResponse>(msg);
            Debug.Log($"HandleMatchStarted - Trends: {response.Trends}");

            MatchDataManager.Instance.ReceivedTrends(response.Trends);

            GameManager.Instance.StartGame();
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
            Debug.Log($"HandleMessage: {msg}");
            DispatchMessage(msg);
        }

        private void DispatchMessage(string msg)
        {
            BaseResponse baseMsg = JsonConvert.DeserializeObject<BaseResponse>(msg);
            Debug.Log($"DispatchMessage: {baseMsg}");
            MessageType type = (MessageType)Enum.Parse(typeof(MessageType), baseMsg.Type);
            switch (type)
            {
                case MessageType.GetRooms:
                    Debug.Log($"DispatchMessage {type} case...");
                    HandleRoomListReceived(msg);
                    break;
                case MessageType.JoinRoom:
                    Debug.Log($"DispatchMessage {type} case...");
                    HandleRoomJoined(msg);
                    break;
                case MessageType.GameStarted:
                    Debug.Log($"DispatchMessage {type} case...");
                    HandleMatchStarted(msg);
                    break;
            }
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

        public RoomDTO[] GetRooms()
        {
            return _rooms.ToArray();
        }

        public RoomDTO GetJoinedRoom()
        {
            return _joinedRoom;
        }
    }
}