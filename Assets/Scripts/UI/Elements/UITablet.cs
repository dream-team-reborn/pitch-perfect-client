using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PitchPerfect.UI
{
    public class UITablet : MonoBehaviour
    {
        [SerializeField] private GameObject _trendTab;
        [SerializeField] private Image _trendButtonBg;
        [SerializeField] private GameObject _voteTab;
        [SerializeField] private Image _voteButtonBg;

        private void Start()
        {
            SwitchTab(true);
        }

        private void SwitchTab(bool isTrendTab)
        {
            //_trendTab.SetActive(isTrendTab);
            _trendButtonBg.enabled = isTrendTab;
            _voteTab.SetActive(!isTrendTab);
            _voteButtonBg.enabled = !isTrendTab;
        }
        
        public void OnGreenlightClick()
        {
            
        }

        public void OnApprovedClick()
        {
            
        }

        public void OnDeniedClick()
        {
            
        }
    }
}