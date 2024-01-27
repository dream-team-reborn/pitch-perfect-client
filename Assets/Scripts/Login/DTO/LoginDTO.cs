using Newtonsoft.Json;

namespace PitchPerfect.Login.DTO
{
    public class LoginDTO
    {
        private string name;
        private string token;

        public LoginDTO(string name, string token)
        {
            this.name = name;
            this.token = token;
        }

        public override string ToString()
        {
            return $"Name: {name} - Token: {token}";
        }

        public string ConvertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}