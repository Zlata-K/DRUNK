using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private DrunkMeter drunkMeter;
        [SerializeField] private Multiplier multiplier;
        [SerializeField] private TMP_Text score;
        [SerializeField] private TMP_Text hp;
        [SerializeField] private GameObject effectMeterPrefab;
        [SerializeField] private GameObject effectMeterList;
        
        private Dictionary<string, EffectMeter> _effects = new Dictionary<string, EffectMeter>();
        private const float PosOffset = 50.0f;
        void Start()
        {
            RefreshUI();
        }

        private void Update()
        {
            UpdateEffects();
        }

        public void RefreshUI()
        {
            drunkMeter.SetDrunkValue();
            multiplier.SetMultiplierValue();
            score.text = Indestructibles.PlayerData.CurrentScore.ToString();
            hp.text = Indestructibles.PlayerData.HealthPoints.ToString();
        }

        public void AddEffect(string effectName, Color effectColor, float effectDuration)
        {
            if (_effects.ContainsKey(effectName))
            {
                _effects[effectName].SetDuration(effectDuration);
                return;
            }
            var newEffect = Instantiate(effectMeterPrefab, effectMeterList.gameObject.transform).GetComponent<EffectMeter>();
            newEffect.Setup(effectColor, effectDuration);
            _effects.Add(effectName, newEffect);
            UpdateEffectPositions();
        }

        public void UpdateEffects()
        {
            foreach (var effect in _effects)
            {
                if (effect.Value.IsOver())
                {
                    _effects.Remove(effect.Key);
                    Destroy(effect.Value.gameObject);
                    UpdateEffectPositions();
                    return;
                }
            }
        }

        private void UpdateEffectPositions()
        {
            Vector3 currentPos = new Vector3();
            foreach (var effect in _effects)
            {
                effect.Value.rectTransform.localPosition = currentPos;
                currentPos.y -= PosOffset;
            }
        }
    }
}