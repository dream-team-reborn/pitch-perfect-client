using Newtonsoft.Json;

namespace PitchPerfect.Login.DTO
{
    public class LoginDTO
    {
        public string Name;
        public string Token;

        public LoginDTO(string name, string token)
        {
            Name = name;
            Token = token;
        }

        public override string ToString()
        {
            return $"Name: {Name} - Token: {Token}";
        }

        public string ConvertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}