using System.Collections.Generic;
using System.Linq;

namespace PitchPerfect.DTO
{
    public class RoomDTO
    {
        private string _id;
        public string Id => _id;

        private string _name;
        public string Name => _name;

        private List<PlayerDTO> _players;
        public List<PlayerDTO> Players => _players;

        public PlayerDTO GetRoomPlayerById(string id)
        {
            foreach(var player in _players)
            {
                if(player.Id.Equals(id))
                {
                    return player;
                }
            }
            return null;
        }
        
        public RoomDTO(string id, string name, List<PlayerDTO> players)
        {
            _id = id;
            _name = name;
            _players = players;
        }
    }
}