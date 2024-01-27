using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private GameObject _readyText;
        
        private int _id;
        public int Id => _id;

        public void Setup(int id, string username)
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