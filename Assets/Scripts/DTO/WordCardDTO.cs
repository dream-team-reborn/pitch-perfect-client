
namespace PitchPerfect.DTO
{
    public class WordCardDTO : MetaEntityDTO
    {
        int categoryId;
        public int CategoryId => categoryId;

        public WordCardCategoryDTO Category; //TODO Need to retrieve it from manager

        public WordCardDTO(int id, string localizationKey, int categoryId) : base(id, localizationKey)
        {
            this.categoryId = categoryId;
        }
    }
}
