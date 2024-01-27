using System.Collections.Generic;
using PitchPerfect.Networking.ServerEntities;

namespace PitchPerfect.Networking.Responses
{
    public class GetRoomsResponse
    {
        public string Type;

        public List<RoomEntity> Rooms;
        
    }
}