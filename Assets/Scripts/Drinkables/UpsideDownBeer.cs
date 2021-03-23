using Structs;
using UnityEngine;

namespace Drinkables
{
    public class UpsideDownBeer : AbstractDrinkable
    {
        public override void Drink()
        {
            EffectTimer = 15.0f;
            Indestructibles.Controls.InverseKeys();
            PlayerData.ScoreMultiplier *= 2;
            CommonDrunkennessEffects();
        }

        public override void StopDrinkingEffect()
        {
            Indestructibles.Controls.InverseKeys();
            PlayerData.ScoreMultiplier /= 2;
        }
    }
}