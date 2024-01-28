
using System.Collections.Generic;
using PitchPerfect.Networking.ServerEntities;

namespace PitchPerfect.Networking.Responses
{
    public class TurnEndedResponse
    {
        public string Type;
        public Dictionary<string, int> Trends;
        public Dictionary<string, int> Result;
        public Dictionary<string, int> Leaderboards;
        public bool LastTurn;
    }
}