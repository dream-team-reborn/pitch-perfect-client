
using System.Collections.Generic;
using PitchPerfect.Networking.ServerEntities;

namespace PitchPerfect.Networking.Responses
{
    public class TurnStartedResponse
    {
        public string Type;
        public List<CardEntity> Cards;
        public PhraseEntity Phrase;
    }
}