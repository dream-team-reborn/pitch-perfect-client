using System;
using UnityEngine;

namespace PitchPerfect.Database
{
    [CreateAssetMenu(menuName = "Databases/ColorsDatabase", fileName = "ColorsDatabase")]
    public class ColorsDatabase : ScriptableObject
    {
        [Serializable]
        private struct ColorResource
        {
            public int Id;
            public Color Color;
        }
        
        [SerializeField] private ColorResource[] _colors;
        
        public Color GetColor(int id)
        {
            foreach (var color in _colors)
            {
                if (color.Id == id)
                {
                    return color.Color;
                }
            }

            return Color.white;
        }
    }
}