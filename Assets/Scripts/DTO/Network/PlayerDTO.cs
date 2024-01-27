namespace PitchPerfect.DTO
{
    public class PlayerDTO
    {
        private string _id;
        public string Id => _id;

        private string _username;
        public string Username => _username;

        public PlayerDTO(string id, string username)
        {
            _id = id;
            _username = username;
        }
    }
}