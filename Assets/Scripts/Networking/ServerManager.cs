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
        [SerializeField]
        public bool UseLocal = false;

        private void OnDisable()
        {
            if(_socketHandler.IsConnectionOpen())
            {
                SendLeaveRoom();
            }
            _socketHandler.Dispose();
        }

        static string CONFIG_ENDPOINT = "http://[2a03:b0c0:3:d0::76d:d001]:8080/config";
        static string CONFIG_ENDPOINT_LOCAL = "http://192.168.1.201:8080/config";

        static string POST_LOGIN_ENDPOINT = "http://[2a03:b0c0:3:d0::76d:d001]:8080/login";
        static string POST_LOGIN_ENDPOINT_LOCAL = "http://192.168.1.201:8080/login";

        static string WEBSOCKET_ENDPOINT = "ws://[2a03:b0c0:3:d0::76d:d001]:8080/ws?token=$";
        static string WEBSOCKET_ENDPOINT_LOCAL = "ws://192.168.1.201:8080/ws?token=$";

        private SocketHandler _socketHandler;

        AuthorizedUserDTO _authorizedUser = null;
        public AuthorizedUserDTO Player => _authorizedUser;

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
            using (UnityWebRequest getRequest = UnityWebRequest.Get(UseLocal ? CONFIG_ENDPOINT_LOCAL : CONFIG_ENDPOINT))
            {
                yield return getRequest.SendWebRequest();

                if (getRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(getRequest.error);
                    yield break;
                }
                else
                {
                    CardDataManager.Instance.OnConfigReceived(getRequest.downloadHandler.text);
                }
            }
            Debug.Log("Retrieving authorization token... endpoint: " + POST_LOGIN_ENDPOINT);
            string jsonContent = JsonConvert.SerializeObject(new LoginDTO(username, "").ConvertToJson());

            using (UnityWebRequest postRequest = UnityWebRequest.Post(UseLocal ? POST_LOGIN_ENDPOINT_LOCAL : POST_LOGIN_ENDPOINT, jsonContent, "application/json"))
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
            string endPoint = UseLocal ? WEBSOCKET_ENDPOINT_LOCAL : WEBSOCKET_ENDPOINT;
            _socketHandler = new SocketHandler(endPoint.Replace("$",_authorizedUser.Token));
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

        public void SendCreateRoomRequest(string roomName)
        {
            string message = new CreateRoomMessage(roomName).ConvertToJson();
            Debug.Log("Sending message: " + message);
            _socketHandler.Send(message);
        }

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

        private void HandleUserJoined(string msg)
        {
            RoomJoinedResponse response = JsonConvert.DeserializeObject<RoomJoinedResponse>(msg);
            Debug.Log($"HandleUserJoined - Name: {response.Player.Name}");

            MatchDataManager.Instance.ReceivedPlayer(new PlayerDTO(response.Player.ID, response.Player.Name));
        }

        private void HandlePlayerReady()
        {

        }

        private void HandleMatchStarted(string msg)
        {
            GameStartedResponse response = JsonConvert.DeserializeObject<GameStartedResponse>(msg);
            Debug.Log($"HandleMatchStarted - Trends: {response.Trends}");

            MatchDataManager.Instance.ReceivedTrends(response.Trends);
        }

        private void HandleTurnStarted(string msg)
        {
            TurnStartedResponse response = JsonConvert.DeserializeObject<TurnStartedResponse>(msg);
            Debug.Log($"HandleTurnStarted - Cards: {response.Cards.Count}");

            MatchDataManager.Instance.SetCurrentPhrase(CardDataManager.Instance.GetPhraseCardById(response.Phrase.ID));
            List<int> idsOfCards = response.Cards.Select(o => o.ID).ToList();
            MatchDataManager.Instance.ReceivedCards(CardDataManager.Instance.GetWordCardListByIds(idsOfCards));

            GameManager.Instance.StartGame();
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
                case MessageType.TurnStarted:
                    Debug.Log($"DispatchMessage {type} case...");
                    HandleTurnStarted(msg);
                    break;
                case MessageType.CreateRoom:
                    SendListRoomRequest();
                    break;
                case MessageType.RoomJoined:
                    HandleUserJoined(msg);
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