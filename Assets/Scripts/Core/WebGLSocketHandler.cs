
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

                    OnHandleMessage?.Invoke(stringMsg);
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
