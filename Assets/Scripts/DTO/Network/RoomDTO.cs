namespace PitchPerfect.DTO
{
    public class RoomDTO
    {
        private int _id;
        public int Id => _id;

        private string _name;
        public string Name => _name;
        
        public RoomDTO(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}