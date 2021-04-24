using UnityEngine;

namespace Drinkables
{
    public class RegularBeer : AbstractDrinkable
    {
        
        private void Awake()
        {
            Drink();
            Invoke($"SoberUp", Indestructibles.SoberingTime);
        }
        
        protected override void Drink()
        {
            Indestructibles.PlayerData.CurrentScore += 20 * Indestructibles.PlayerData.ScoreMultiplier;
            CommonDrunkennessEffects();
        }

        protected override void StopDrinkingEffect()
        {
            // no effect
        }
    }
}
