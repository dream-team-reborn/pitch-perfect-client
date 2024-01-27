using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace PitchPerfect.UI
{
    public class UIRoom : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private GameObject _selectedText;

        private int _id;
        private UIRoomsPage _roomsPage;

        public int Id => _id;
        
        public void Setup(int id, string name, UIRoomsPage roomsPage)
        {
            _id = id;
            _roomsPage = roomsPage;

            _nameText.text = name;
            Deselect();
        }

        public void OnSelect()
        {
            _selectedText.SetActive(true);
            _roomsPage.SelectRoom(_id);
        }

        public void Deselect()
        {
            _selectedText.SetActive(false);
        }
    }
}