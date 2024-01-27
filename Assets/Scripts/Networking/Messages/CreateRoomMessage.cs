
namespace PitchPerfect.Networking.Messages
{
    public class CreateRoomMessage : BaseMessage
    {
        public CreateRoomMessage() : base(MessageType.CreateRoom)
        {
        }
    }
}
