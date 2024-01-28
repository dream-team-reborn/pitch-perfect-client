using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PitchPerfect.UI
{
    public class UIFader : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [FormerlySerializedAs("fadeDuration")] [SerializeField] private float _fadeDuration;
        
        private bool _fade;
        private float _fadeStartT;
        private float _currentFadeT;
        private float _startingAlpha;

        private void Start()
        {
            _image.material = new Material(_image.material);
        }

        public void Fade()
        {
            _fade = true;
            _fadeStartT = Time.time;
            _startingAlpha = _image.material.color.a;
        }

        private void Update()
        {
            if (!_fade)
                return;

            float t = (Time.time - _fadeStartT) / _fadeDuration;
            float alpha = _startingAlpha > 0f ? Mathf.Lerp(_startingAlpha, 0f, t) : Mathf.Lerp(0f, 1f, t);
            _image.material.color = new Color(1, 1, 1, alpha);

            if (t >= 1f)
            {
                _fade = false;
            }
        }
    }
}