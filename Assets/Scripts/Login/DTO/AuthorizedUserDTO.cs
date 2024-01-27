namespace PitchPerfect.Login.DTO
{
    public class AuthorizedUserDTO
    {
        public string UserId;
        public string Token;

        public override string ToString()
        {
            return $"UserId: {UserId} - Token: {Token}";
        }
    }
}
