using System.Collections.Generic;

namespace PitchPerfect.Networking.Responses
{
    public class AllPlayerSelectedCardsResponse
    {
        public string Type;
        public Dictionary<string, List<int>> PlayersCards;
    }
}
