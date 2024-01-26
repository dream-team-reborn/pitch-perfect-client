using System;
using UnityEngine;

namespace PitchPerfect.Database
{
    [CreateAssetMenu(menuName = "Databases/SpritesDatabase", fileName = "SpritesDatabase")]
    public class SpritesDatabase : ScriptableObject
    {
        [Serializable]
        private struct SpriteResource
        {
            public int ID;
            public Sprite Sprite;
        }
        
        [SerializeField] private SpriteResource[] sprites;

        public Sprite GetResource(int id)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.ID == id)
                {
                    return sprite.Sprite;
                }
            }

            return null;
        }
    }
}