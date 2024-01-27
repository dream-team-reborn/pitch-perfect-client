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
        private UIRoomsListPage _roomsListPage;

        public int Id => _id;
        
        public void Setup(int id, string name, UIRoomsListPage roomsListPage)
        {
            _id = id;
            _roomsListPage = roomsListPage;

            _nameText.text = name;
            Deselect();
        }

        public void OnSelect()
        {
            _selectedText.SetActive(true);
            _roomsListPage.SelectRoom(_id);
        }

        public void Deselect()
        {
            _selectedText.SetActive(false);
        }
    }
}