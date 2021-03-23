using UnityEngine;

namespace Drinkables
{
    public class ClearlyThereBeer : AbstractDrinkable
    {
        
        Renderer renderer = GameObject.Find("player").GetComponent<Renderer>();
        
        public override void Drink()
        {
            renderer.enabled = false;
            PlayerData.ScoreMultiplier /= 4;
        }

        public override void StopDrinkingEffect()
        {
            renderer.enabled = true;
            PlayerData.ScoreMultiplier *= 4;
        }
    }
}