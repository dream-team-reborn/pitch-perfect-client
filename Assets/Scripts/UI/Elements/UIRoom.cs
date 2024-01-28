using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace PitchPerfect.UI
{
    public class UIRoom : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _playersAmount;
        [SerializeField] private GameObject _selectedText;

        private string _id;
        private UIRoomsListPage _roomsListPage;

        public string Id => _id;
        
        public void Setup(string id, string name, int playersAmount, UIRoomsListPage roomsListPage)
        {
            _id = id;
            _roomsListPage = roomsListPage;

            _playersAmount.text = playersAmount.ToString();
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