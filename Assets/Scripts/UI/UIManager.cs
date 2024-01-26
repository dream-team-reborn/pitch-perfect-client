using com.trashpandaboy.core;
using PitchPerfect.Database;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIManager : Manager<UIManager>
    {
        [SerializeField] private UIPage[] _pages;

        [SerializeField] private SpritesDatabase _spritesDatabase;

        public SpritesDatabase SpritesDatabase => _spritesDatabase;

        public void Show<T>()
        {
            foreach (var page in _pages)
            {
                if (page is T)
                {
                    page.Show();
                    return;
                }
            }
        }

        public void Hide<T>()
        {
            foreach (var page in _pages)
            {
                if (page is T)
                {
                    page.Hide();
                    return;
                }
            }
        }
    }
}