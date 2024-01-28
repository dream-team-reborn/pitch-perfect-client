using PitchPerfect.Networking;

namespace PitchPerfect.Networking.Messages
{
    public class PlayerReadyMessage : BaseMessage
    {
        public string RoomId => _roomId;
        private string _roomId;

        public PlayerReadyMessage(string roomId) : base(MessageType.PlayerReady)
        {
            _roomId = roomId;
        }
    }
}
