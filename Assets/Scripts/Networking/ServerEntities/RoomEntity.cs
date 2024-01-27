using System.Collections.Generic;
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
            return new RoomDTO(ID, Name);
        }
    }
}