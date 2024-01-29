
using System;
using System.Collections;
using System.Text;
using HybridWebSocket;
using UnityEngine;

namespace PitchPerfect.Core
{
    public class WebGLSocketHandler
    {
        WebSocket wsWebGL;

        public Action<string> OnHandleMessage;

        public WebGLSocketHandler(string endPoint)
        {
            wsWebGL = WebSocketFactory.CreateInstance(endPoint);

            wsWebGL.OnOpen += () =>
            {
                Debug.Log("WS connected!");
                Debug.Log("WS state: " + wsWebGL.GetState().ToString());

            };

            // Add OnMessage event listener
            wsWebGL.OnMessage += (byte[] msg) =>
            {
                Debug.Log($"Received {msg.Length} bytes");
                try
                {
                    string stringMsg = Encoding.UTF8.GetString(msg);
                    Debug.Log("WS received message: " + stringMsg);

                    OnHandleMessage?.Invoke(stringMsg);
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
            };

            // Add OnError event listener
            wsWebGL.OnError += (string errMsg) =>
            {
                Debug.Log("WS error: " + errMsg);
            };

            // Add OnClose event listener
            wsWebGL.OnClose += (WebSocketCloseCode code) =>
            {
                Debug.Log("WS closed with code: " + code.ToString());
            };

            // Connect to the server
            wsWebGL.Connect();
        }

        public void SendMessage(string msg)
        {
            if (wsWebGL.GetState() == WebSocketState.Open)
                wsWebGL.Send(Encoding.UTF8.GetBytes(msg));
        }

        public bool IsConnectionOpen()
        {
            return wsWebGL.GetState() == WebSocketState.Open;
        }

        IEnumerator OnHandleMessageCoroutine()
        {
            yield break;
        }
    }
}
