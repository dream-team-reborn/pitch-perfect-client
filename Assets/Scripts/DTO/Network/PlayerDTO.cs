namespace PitchPerfect.DTO
{
    public class PlayerDTO
    {
        private int _id;
        public int Id => _id;

        private string _username;
        public string Username => _username;

        public PlayerDTO(int id, string username)
        {
            _id = id;
            _username = username;
        }
    }
}