using PitchPerfect.DTO;

namespace PitchPerfect.Networking.ServerEntities
{
    public class PlayerEntity
    {
        public string ID;
        public string Name;
        public string RoomId;

        public PlayerDTO ConvertToDTO()
        {
            return new PlayerDTO(ID, Name);
        }
    }
}