using Newtonsoft.Json;

namespace PitchPerfect.Networking.Messages
{
    public class BaseMessage
    {
        public string Type => _type.ToString();
        
        [JsonIgnore]
        public MessageType TypeEnumValue => _type;
        private MessageType _type;

        public BaseMessage(MessageType type)
        {
            _type = type;
        }

        public string ConvertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class LapisMessage : BaseMessage
    {
        public LapisMessage() : base(MessageType.Lapis)
        {

        }
    }
}
