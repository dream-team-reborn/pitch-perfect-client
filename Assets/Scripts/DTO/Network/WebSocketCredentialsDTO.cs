
namespace PitchPerfect.Login.DTO
{
    public class WebSocketCredentialsDTO
    {
        private string username;
        private string password;

        public string Username => username;
        public string Password => password;

        public WebSocketCredentialsDTO(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

    }
}
