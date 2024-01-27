using System.Collections;
using Newtonsoft.Json;
using PitchPerfect.Login.DTO;
using PitchPerfect.UI;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;

namespace PitchPerfect.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        static string POST_LOGIN_ENDPOINT = "http://[2a03:b0c0:3:d0::76d:d001]:8080/login";
        static string GET_WEBSOCKET_CREDENTIALS_ENDPOINT = "http://[2a03:b0c0:3:d0::76d:d001]:8080/ws";
        static string WEBSOCKET_ENDPOINT = "";

        WebSocket _webSocket = null;
        AuthorizedUserDTO _authorizedUser = null;
        WebSocketCredentialsDTO _webSocketCredentials = null;

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
                    StartCoroutine(GetCredentials());
                }
            }
        }

        IEnumerator GetCredentials()
        {
            Debug.Log("Retrieving websocket credentials...");
            using (UnityWebRequest getRequest = UnityWebRequest.Get(GET_WEBSOCKET_CREDENTIALS_ENDPOINT))
            {
                getRequest.SetRequestHeader("Token", _authorizedUser.Token);

                yield return getRequest.SendWebRequest();

                if (getRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(getRequest.error);
                }
                else
                {
                    Debug.Log($"Credentials retrieved: \n {getRequest.downloadHandler.text}");
                    //OpenWebSocket(); - when retrieved credential store them and uncomment
                }
            }
        }

        private void OpenWebSocket()
        {
            Debug.Log("Called OpenWebSocket");

            _webSocket = new WebSocket(WEBSOCKET_ENDPOINT);
            _webSocket.SetCredentials(_webSocketCredentials.Username,_webSocketCredentials.Password, true);
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _webSocket.Send("Test Echo");
            }
        }

    }
}