using TMPro;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private GameObject _readyText;
        
        private string _id;
        public string Id => _id;

        public void Setup(string id, string username)
        {
            _id = id;
            _nameText.text = username;

            _readyText.SetActive(false);
        }

        public void OnPlayerReady()
        {
            _readyText.SetActive(true);
        }
    }
}