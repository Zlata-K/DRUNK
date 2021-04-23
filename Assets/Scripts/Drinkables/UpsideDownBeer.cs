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
        }

        protected override void Drink()
        {
            Indestructibles.PlayerData.UpsideDownStack++;

            // Only inverse if this is the only `Upside Down` beer in the player's belly
            if (Indestructibles.PlayerData.UpsideDownStack == 1)
            {
                Indestructibles.MovementControls.InverseKeys();
            }

            Indestructibles.PlayerData.ScoreMultiplier *= 2;
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

            Indestructibles.PlayerData.ScoreMultiplier /= 2;
        }
    }
}