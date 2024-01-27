using UnityEngine;

namespace PitchPerfect.Database
{
    [CreateAssetMenu(menuName = "Databases/SpritesDatabase", fileName = "SpritesDatabase")]
    public class SpritesDatabase : ScriptableObject
    {
        [SerializeField] private Sprite[] sprites;

        public Sprite GetResource(string key)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.name == key)
                {
                    return sprite;
                }
            }

            return null;
        }
    }
}