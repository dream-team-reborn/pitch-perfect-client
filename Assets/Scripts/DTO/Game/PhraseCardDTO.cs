namespace PitchPerfect.DTO
{
    public class PhraseCardDTO : MetaEntityDTO
    {
        int placeholderAmount;
        public int PlaceholderAmount => placeholderAmount;
     
        public PhraseCardDTO(int id, string localizationKey, int placeholderAmount) : base(id, localizationKey)
        {
            this.placeholderAmount = placeholderAmount;
        }
    }
}