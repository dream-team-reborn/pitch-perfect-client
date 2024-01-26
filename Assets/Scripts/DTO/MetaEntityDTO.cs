using Core;

namespace DTO
{
    public class MetaEntityDTO : ILocalizedEntity
    {
        int id;
        public int Id => id;

        string localizationKey;
        public string LocalizationKey => localizationKey;

        public MetaEntityDTO(int id, string localizationKey)
        {
            this.id = id;
            this.localizationKey = localizationKey;
        }

        public string GetLocalizedContent()
        {
            return LocalizationManager.Instance.GetLocalizedString(localizationKey);
        }
    }
}
