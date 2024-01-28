
using System.Collections.Generic;
using UnityEngine;

namespace PitchPerfect.Networking.Responses
{
    public class GameStartedResponse : MonoBehaviour
    {
        public string Type;
        public Dictionary<string, int> Trends;
    }
}
