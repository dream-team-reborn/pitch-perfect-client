
using System.Collections.Generic;

namespace PitchPerfect.Networking.Messages
{
    public class PlayerCardSelectedRequest : BaseMessage
    {
        public string PlayerId => _playerId;
        private string _playerId;

        public List<int> Cards => _cards;
        private List<int> _cards;


        public PlayerCardSelectedRequest(string playerId, List<int> cards) : base(MessageType.PlayerReady)
        {
            _playerId = playerId;
            _cards = cards;
        }
    }
}