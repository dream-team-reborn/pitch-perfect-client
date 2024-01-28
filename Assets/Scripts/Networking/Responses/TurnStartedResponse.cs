
using System.Collections.Generic;
using PitchPerfect.Networking.ServerEntities;

namespace PitchPerfect.Networking.Responses
{
    public class TurnStartedResponse
    {
        //HandleMessage: {"Cards":[{"ID":1,"CategoryId":4},{ "ID":27,"CategoryId":4},{ "ID":23,"CategoryId":1},{ "ID":20,"CategoryId":1}],"Phrase":{ "ID":16,"PlaceholdersAmount":1},"Type":"TurnStarted"}
        public string Type;
        public List<CardEntity> Cards;
        public PhraseEntity Phrase;
    }
}