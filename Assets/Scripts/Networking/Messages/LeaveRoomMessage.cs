using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PitchPerfect.Networking.Messages
{
    public class LeaveRoomMessage : BaseMessage
    {
        public string RoomId => _roomId;
        private string _roomId;

        public LeaveRoomMessage(string roomId) : base(MessageType.LeaveRoom)
        {
            _roomId = roomId;
        }
    }
}