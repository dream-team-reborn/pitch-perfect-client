
using System.Collections.Generic;

namespace PitchPerfect.Networking.Messages
{
    public class PlayerRatedOtherCardsMessage : BaseMessage
    {
        public string RoomId => _roomId;
        private string _roomId;

        public Dictionary<string, bool> Reviews => _reviews;
        private Dictionary<string, bool> _reviews;

        public PlayerRatedOtherCardsMessage(string roomId, Dictionary<string, bool> votes) : base(MessageType.PlayerRatedOtherCards)
        {
            _roomId = roomId;
            _reviews = votes;
        }
    }
}
