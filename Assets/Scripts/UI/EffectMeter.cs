using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EffectMeter : MonoBehaviour
    {
        public RectTransform rectTransform;
        public float duration;

        private float _currentTime;
        private Slider _slider;
        private Image _fill;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            _slider = GetComponent<Slider>();
            _fill = GetComponentInChildren<Image>();
        }

        void Update()
        {
            _currentTime -= Time.deltaTime;
            _slider.value = _currentTime/duration;
        }
        public void Setup(Color color, float newDuration)
        {
            _fill.color = color;
            SetDuration(newDuration);
        }
        public void SetDuration(float newDuration)
        {
            duration = newDuration;
            _currentTime = duration;
        }

        public bool IsOver()
        {
            return _currentTime <= 0.0f;
        }
    }
}