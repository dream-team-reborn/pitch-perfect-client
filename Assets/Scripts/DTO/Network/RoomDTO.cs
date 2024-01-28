using System.Collections.Generic;

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
        
        public RoomDTO(string id, string name, List<PlayerDTO> players)
        {
            _id = id;
            _name = name;
            _players = players;
        }
    }
}