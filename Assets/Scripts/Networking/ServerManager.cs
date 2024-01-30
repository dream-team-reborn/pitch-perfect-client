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
using HybridWebSocket;
using System.Text;
using PimDeWitte.UnityMainThreadDispatcher;

namespace PitchPerfect.Networking
{
    public class ServerManager : Manager<ServerManager>
    {

        WebSocket wsWebGL;

        [SerializeField]
        public bool UseLocal = false;

        private void OnDisable()
        {
            SendLeaveRoom();
        }

        static string CONFIG_ENDPOINT = "https://pitch-perfect.mstefanini.com/config";
        static string CONFIG_ENDPOINT_LOCAL = "http://192.168.1.201:8080/config";

        static string POST_LOGIN_ENDPOINT = "https://pitch-perfect.mstefanini.com/login";
        static string POST_LOGIN_ENDPOINT_LOCAL = "http://192.168.1.201:8080/login";

        static string WEBSOCKET_ENDPOINT = "wss://pitch-perfect.mstefanini.com/ws?token=$";
        static string WEBSOCKET_ENDPOINT_LOCAL = "ws://192.168.1.201:8080/ws?token=$";

        AuthorizedUserDTO _authorizedUser = null;
        public AuthorizedUserDTO Player => _authorizedUser;

        List<RoomDTO> _rooms = new List<RoomDTO>();
        RoomDTO _joinedRoom = null;

        public Action OnGameStart = null;
        public Action OnTurnStart = null;
        public Action OnAllUsersSelectedCards = null;
        public Action OnAllUsersVoted = null;
        public Action OnTurnEnd = null;
        public Action OnGameEnd = null;

        private void Start()
        {
            UIManager.Instance.Show<UILoginPage>();
        }

        public void RequestLogin(string username)
        {
            //TODO: Send HTTP message with username
            Log.Info($"Requesting Login with username {username}");

            StartCoroutine(SendLoginRequest(username));
        }

        IEnumerator SendLoginRequest(string username)
        {
            using (UnityWebRequest getRequest = UnityWebRequest.Get(UseLocal ? CONFIG_ENDPOINT_LOCAL : CONFIG_ENDPOINT))
            {
                yield return getRequest.SendWebRequest();

                if (getRequest.result != UnityWebRequest.Result.Success)
                {
                    Log.Info(getRequest.error);
                    yield break;
                }
                else
                {
                    CardDataManager.Instance.OnConfigReceived(getRequest.downloadHandler.text);
                }
            }
            Log.Info("Retrieving authorization token... endpoint: " + POST_LOGIN_ENDPOINT);

            var loginDtoJson = new LoginDTO(username, "").ConvertToJson();

            Log.Info("LoginDTO: " + loginDtoJson);

            using (UnityWebRequest postRequest = UnityWebRequest.Post(UseLocal ? POST_LOGIN_ENDPOINT_LOCAL : POST_LOGIN_ENDPOINT, loginDtoJson, "application/json"))
            {
                yield return postRequest.SendWebRequest();

                if (postRequest.result != UnityWebRequest.Result.Success)
                {
                    Log.Info(postRequest.error);
                }
                else
                {
                    _authorizedUser = JsonConvert.DeserializeObject<AuthorizedUserDTO>(postRequest.downloadHandler.text);
                    Log.Info($"Retrieved authorized user \n {_authorizedUser}");
                    StartCoroutine(OpenClientSocket());
                }
            }
        }

        IEnumerator OpenClientSocket()
        {
            string endPoint = UseLocal ? WEBSOCKET_ENDPOINT_LOCAL : WEBSOCKET_ENDPOINT;
            endPoint = endPoint.Replace("$", _authorizedUser.Token);

            CreateAndConnectSocket(endPoint);

            StartCoroutine(RetrieveRoomList());
            yield break;
        }

        private void CreateAndConnectSocket(string endPoint)
        {
            wsWebGL = WebSocketFactory.CreateInstance(endPoint);

            wsWebGL.OnOpen += () =>
            {
                Log.Info("WS connected!");
                Log.Info("WS state: " + wsWebGL.GetState().ToString());

            };

            // Add OnMessage event listener
            wsWebGL.OnMessage += (byte[] msg) =>
            {
                Log.Info($"Received {msg.Length} bytes");
                try
                {
                    string stringMsg = Encoding.UTF8.GetString(msg);
                    Log.Info("WS received message: " + stringMsg);

                    HandleMessageThreadWrapper(stringMsg);
                }
                catch (Exception e)
                {
                    Log.Info(e.StackTrace);
                }
            };

            // Add OnError event listener
            wsWebGL.OnError += (string errMsg) =>
            {
                Log.Info("WS error: " + errMsg);
            };

            // Add OnClose event listener
            wsWebGL.OnClose += (WebSocketCloseCode code) =>
            {
                Log.Info("WS closed with code: " + code.ToString());
            };

            // Connect to the server
            wsWebGL.Connect();
        }

        IEnumerator RetrieveRoomList()
        {
            if (!IsConnectionOpen())
            {
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(RetrieveRoomList());
                yield break;
            }
            
            Log.Info("RetrieveRoomList - IsConnectionOpen: " + IsConnectionOpen());
            SendListRoomRequest();
        }

        private bool IsConnectionOpen()
        {
            return wsWebGL.GetState() == WebSocketState.Open;
        }

        #region Requests

        public void SendCreateRoomRequest(string roomName)
        {
            string message = new CreateRoomMessage(roomName).ConvertToJson();
            Log.Info("Sending message: " + message);
            //_socketHandler.Send(message);
            SendRequest(message);
        }

        public void SendListRoomRequest()
        {
            string message = new GetRoomsMessage().ConvertToJson();
            Log.Info("Sending message: " + message);
            //_socketHandler.Send(message);
            SendRequest(message);
        }

        public void SendJoinRoom(string roomId)
        {
            string message = new JoinRoomMessage(roomId).ConvertToJson();
            Log.Info("Sending message: " + message);
            _joinedRoom = _rooms.Where(o => o.Id.Equals(roomId)).Single();
            //_socketHandler.Send(message);
            SendRequest(message);
        }

        public void SendLeaveRoom()
        {
            if(_joinedRoom != null)
            {
                string message = new LeaveRoomMessage(_joinedRoom.Id).ConvertToJson();
                Log.Info("Sending message: " + message);
                //_socketHandler.Send(message);
                SendRequest(message);
            }
        }

        public void SendPlayerReady()
        {
            string message = new PlayerReadyMessage(_joinedRoom.Id).ConvertToJson();
            Log.Info("Sending message: " + message);
            //_socketHandler.Send(message);
            SendRequest(message);
        }

        public void SendCardSelection()
        {
            string message = new PlayerCardSelectedMessage(_authorizedUser.UserId, _joinedRoom.Id, MatchDataManager.Instance.SelectedCards).ConvertToJson();
            Log.Info("Sending message: " + message);
            //_socketHandler.Send(message);
            SendRequest(message);
        }

        public void SendVoteOfSelection(Dictionary<string ,bool> votes)
        {
            string message = new PlayerRatedOtherCardsMessage(_joinedRoom.Id, votes).ConvertToJson();
            Log.Info("Sending message: " + message);
            //_socketHandler.Send(message);
            SendRequest(message);
        }

        #endregion

        #region Responses

        private void HandleRoomListReceived(string msg)
        {
            GetRoomsResponse response = JsonConvert.DeserializeObject<GetRoomsResponse>(msg);

            _rooms = response.Rooms.Select(o => o.ConvertToDTO()).ToList();

            EventDispatcher.TriggerEvent(GameManager.GameManageEvents.JoinRoomList.ToString());

        }

        private void HandleRoomJoined(string msg)
        {
            JoinRoomResponse response = JsonConvert.DeserializeObject<JoinRoomResponse>(msg);
            Log.Info($"HandleRoomJoined - Result: {response.Result}");

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
            Log.Info($"HandleUserJoined - Name: {response.Player.Name}");

            MatchDataManager.Instance.ReceivedPlayer(new PlayerDTO(response.Player.ID, response.Player.Name));
        }

        private void HandlePlayerReady()
        {

        }

        private void HandleMatchStarted(string msg)
        {
            GameStartedResponse response = JsonConvert.DeserializeObject<GameStartedResponse>(msg);
            Log.Info($"HandleMatchStarted - Trends: {response.Trends}");

            MatchDataManager.Instance.ReceivedTrends(response.Trends);

            GameManager.Instance.StartGame();
        }

        private void HandleTurnStarted(string msg)
        {
            TurnStartedResponse response = JsonConvert.DeserializeObject<TurnStartedResponse>(msg);
            Log.Info($"HandleTurnStarted - Cards: {response.Cards.Count}");

            MatchDataManager.Instance.SetCurrentPhrase(CardDataManager.Instance.GetPhraseCardById(response.Phrase.ID));
            List<int> idsOfCards = response.Cards.Select(o => o.ID).ToList();
            MatchDataManager.Instance.ReceivedCards(CardDataManager.Instance.GetWordCardListByIds(idsOfCards));
            OnTurnStart?.Invoke();
        }

        private void HandleAllPlayersSelectedCards(string msg)
        {
            AllPlayerSelectedCardsResponse response = JsonConvert.DeserializeObject<AllPlayerSelectedCardsResponse>(msg);
            Log.Info($"HandleAllPlayersSelectedCards - PlayersCards: {response.PlayersCards.Count}");

            var selectedCards = response.PlayersCards.Where(o => !o.Key.Equals(_authorizedUser.UserId)).ToList();

            MatchDataManager.Instance.ReceivedPlayersSelection(selectedCards);
        }

        private void HandleTurnEnd(string msg)
        {
            TurnEndedResponse response = JsonConvert.DeserializeObject<TurnEndedResponse>(msg);
            Log.Info($"HandleTurnEnd - LastTurn: {response.LastTurn}");


            MatchDataManager.Instance.ReceivedLeaderboards(response.Leaderboards);
            MatchDataManager.Instance.ReceivedResults(response.Result);
            MatchDataManager.Instance.ReceivedTrends(response.Trends);
            
            // if(response.LastTurn)
                // GameManager.Instance.EndGame();
        }


        #endregion



        private void HandleMessageThreadWrapper(string msg)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(HandleMessage(msg));
        }

        IEnumerator HandleMessage(string msg)
        {
            Log.Info($"HandleMessage: {msg}");
            DispatchMessage(msg);
            yield break;
        }

        private void DispatchMessage(string msg)
        {
            BaseResponse baseMsg = JsonConvert.DeserializeObject<BaseResponse>(msg);
            Log.Info($"DispatchMessage: {baseMsg}");
            MessageType type = (MessageType)Enum.Parse(typeof(MessageType), baseMsg.Type);
            switch (type)
            {
                case MessageType.GetRooms:
                    Log.Info($"DispatchMessage {type} case...");
                    HandleRoomListReceived(msg);
                    break;
                case MessageType.JoinRoom:
                    Log.Info($"DispatchMessage {type} case...");
                    HandleRoomJoined(msg);
                    break;
                case MessageType.GameStarted:
                    Log.Info($"DispatchMessage {type} case...");
                    HandleMatchStarted(msg);
                    break;
                case MessageType.TurnStarted:
                    Log.Info($"DispatchMessage {type} case...");
                    HandleTurnStarted(msg);
                    break;
                case MessageType.CreateRoom:
                    SendListRoomRequest();
                    break;
                case MessageType.RoomJoined:
                    HandleUserJoined(msg);
                    break;
                case MessageType.AllPlayerSelectedCards:
                    HandleAllPlayersSelectedCards(msg);
                    break;
                case MessageType.RoomLeaved:
                    break;
                case MessageType.TurnEnded:
                    HandleTurnEnd(msg);
                    break;
            }
        }

        public void SendRequest(string message)
        {
            if (IsConnectionOpen())
                wsWebGL.Send(Encoding.UTF8.GetBytes(message));
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