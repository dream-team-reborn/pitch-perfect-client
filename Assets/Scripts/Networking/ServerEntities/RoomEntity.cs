using System.Collections.Generic;
using System.Linq;
using PitchPerfect.DTO;

namespace PitchPerfect.Networking.ServerEntities
{
    public class RoomEntity
    {
        public string ID;
        public string Name;
        public List<PlayerEntity> Players;

        public RoomDTO ConvertToDTO()
        {
            return new RoomDTO(ID, Name, Players.Select(o => o.ConvertToDTO()).ToList());
        }
    }
}