using UnityEngine;

namespace PitchPerfect.Utils
{
    public class PositionTween : MonoBehaviour
    {
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private float _duration;
        [SerializeField] private bool _isRelativeToStart;
        [SerializeField] private bool _playOnStart;
        [SerializeField] private bool _isUI;

        private Vector3 _startPosition;
        
        private Vector3 _tweenStartPos;
        private Vector3 _tweenEndPos;
        private bool _play;
        
        private float _startPlayTime;
        
        void Start()
        {
            _startPosition = _isUI ? (transform as RectTransform).anchoredPosition : transform.position;
           
            if (_playOnStart)
            {
                Play();
            }
        }

        public void Play(bool reverse = false, bool fromCurrentPos = false)
        {
            _play = true;
            _startPlayTime = Time.time;
            if (fromCurrentPos)
            {
                _tweenStartPos = _isUI ? (transform as RectTransform).anchoredPosition : transform.position;
            }
            else
            {
                _tweenStartPos = reverse ? GetEndPosition() : _startPosition;
            }
            _tweenEndPos = reverse ? _startPosition : GetEndPosition();
        }

        public void Stop()
        {
            _play = false;
        }

        void Update()
        {
            if (_play)
            {
                var newPos = Vector3.Lerp(_tweenStartPos, _tweenEndPos, (Time.time - _startPlayTime) / _duration);

                if (_isUI)
                {
                    (transform as RectTransform).anchoredPosition = newPos;
                }
                else
                {
                    transform.position = newPos;
                }
                
                CheckPosition();
            }
        }

        private void CheckPosition()
        {
            if (_isUI)
            {
                if (Vector3.Distance((transform as RectTransform).anchoredPosition, _tweenEndPos) < Mathf.Epsilon)
                {
                    (transform as RectTransform).anchoredPosition = _tweenEndPos;
                    _play = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, _tweenEndPos) < Mathf.Epsilon)
                {
                    transform.position = _tweenEndPos;
                    _play = false;
                }
            }
        }

        private Vector3 GetEndPosition()
        {
            if (_isRelativeToStart)
            {
                if (_isUI)
                {
                    return (transform as RectTransform).anchoredPosition + (Vector2)_endPosition;
                }
                else
                {
                    return transform.position + _endPosition;
                }
            }

            return _endPosition;
        }
    }
}