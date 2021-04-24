using UnityEngine;

namespace Drinkables
{
    public class UpsideDownBeer : AbstractDrinkable
    {
        private void Awake()
        {
            Drink();
            Invoke($"StopDrinkingEffect", 15.0f);
            Invoke($"SoberUp", Indestructibles.SoberingTime);
            Indestructibles.UIManager.AddEffect("UpsideDown", Color.red, 15.0f);
        }

        protected override void Drink()
        {
            Indestructibles.PlayerData.UpsideDownStack++;

            // Only inverse if this is the only `Upside Down` beer in the player's belly
            if (Indestructibles.PlayerData.UpsideDownStack == 1)
            {
                Indestructibles.MovementControls.InverseKeys();
            }

            Indestructibles.PlayerData.CurrentScore += 40 * Indestructibles.PlayerData.ScoreMultiplier;
            CommonDrunkennessEffects();
        }

        protected override void StopDrinkingEffect()
        {
            // Only go back to normal if this is the last `Upside Down` beer in the 
            // player's belly
            if (Indestructibles.PlayerData.UpsideDownStack == 1)
            {
                Indestructibles.MovementControls.InverseKeys();
            }

            Indestructibles.PlayerData.UpsideDownStack--;
        }
    }
}