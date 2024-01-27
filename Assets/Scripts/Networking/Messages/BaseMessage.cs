
using Newtonsoft.Json;

namespace PitchPerfect.Networking.Messages
{
    public class BaseMessage
    {
        public string Type => _type.ToString();
        private MessageType _type;

        public BaseMessage(MessageType type)
        {
            _type = type;
        }

        public enum MessageType
        {
            CreateRoom,
            GetRooms
        }

        public string ConvertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
