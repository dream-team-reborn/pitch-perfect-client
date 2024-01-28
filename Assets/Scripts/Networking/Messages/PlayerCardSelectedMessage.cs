
using System.Collections.Generic;

namespace PitchPerfect.Networking.Messages
{
    public class PlayerCardSelectedMessage : BaseMessage
    {
        public string PlayerId => _playerId;
        private string _playerId;

        public string RoomId => _roomId;
        private string _roomId;

        public List<int> Cards => _cards;
        private List<int> _cards;


        public PlayerCardSelectedMessage(string playerId, string roomId, List<int> cards) : base(MessageType.PlayerCardsSelected)
        {
            _playerId = playerId;
            _roomId = roomId;
            _cards = cards;
        }
    }
}