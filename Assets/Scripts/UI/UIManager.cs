using com.trashpandaboy.core;
using PitchPerfect.Database;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIManager : Manager<UIManager>
    {
        [SerializeField] private UIPage[] _pages;
        [SerializeField] private UIPopup[] _popups;

        [SerializeField] private UIFader _fader;
        [SerializeField] private SpritesDatabase _spritesDatabase;
        [SerializeField] private ColorsDatabase _colorsDatabase;
        public UIFader Fader => _fader;
        public SpritesDatabase SpritesDatabase => _spritesDatabase;
        public ColorsDatabase ColorsDatabase => _colorsDatabase;

        private UIPage _currentShownPage;

        public void Show<T>()
        {
            foreach (var page in _pages)
            {
                if (page is T)
                {
                    if (_currentShownPage)
                    {
                        _currentShownPage.Hide();
                    }
                    page.Show();
                    _currentShownPage = page;
                    return;
                }
            }
            
            foreach (var popup in _popups)
            {
                if (popup is T)
                {
                    popup.Show();
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
                    _currentShownPage = null;
                    return;
                }
            }
            
            foreach (var popup in _popups)
            {
                if (popup is T)
                {
                    popup.Hide();
                    return;
                }
            }
        }
    }
}