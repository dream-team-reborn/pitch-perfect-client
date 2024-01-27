
using PitchPerfect.Core;

namespace PitchPerfect.DTO
{
    public class WordCardDTO : MetaEntityDTO
    {
        int categoryId;
        public int CategoryId => categoryId;

        public WordCardCategoryDTO Category => CardDataManager.Instance.GetCardCategoryById(categoryId);

        public WordCardDTO(int id, string localizationKey, int categoryId) : base(id, localizationKey)
        {
            this.categoryId = categoryId;
        }
    }
}
