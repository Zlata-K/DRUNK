using TMPro;
using UnityEngine;

namespace UI
{
    public class Multiplier : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        
        public void SetMultiplierValue()
        {
            text.text = "x" + Indestructibles.PlayerData.ScoreMultiplier;
        }
    }
}