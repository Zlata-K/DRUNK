using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DrunkMeter : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;

        public void SetDrunkValue()
        {
            slider.value = Indestructibles.PlayerData.IntoxicationLevel;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}