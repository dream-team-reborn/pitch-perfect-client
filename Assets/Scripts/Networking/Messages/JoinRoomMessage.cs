using PitchPerfect.Networking;

namespace PitchPerfect.Networking.Messages
{
    public class JoinRoomMessage : BaseMessage
    {
        public string RoomId => _roomId;
        private string _roomId;

        public JoinRoomMessage(string roomId) : base(MessageType.JoinRoom)
        {
            _roomId = roomId;
        }
    }
}
