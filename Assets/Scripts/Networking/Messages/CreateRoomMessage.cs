
namespace PitchPerfect.Networking.Messages
{
    public class CreateRoomMessage : BaseMessage
    {
        public string RoomName => _roomName;
        private string _roomName;

        public CreateRoomMessage(string roomName) : base(MessageType.CreateRoom)
        {
            _roomName = roomName;
        }
    }
}
