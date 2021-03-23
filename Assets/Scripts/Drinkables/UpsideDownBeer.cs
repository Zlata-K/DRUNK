using Structs;
using UnityEngine;

namespace Drinkables
{
    public class UpsideDownBeer : AbstractDrinkable
    {
        public override void Drink()
        {
            EffectTimer = 15.0f;
            Indestructibles.Controls = 
                new KeyControls(KeyCode.S, KeyCode.W, KeyCode.A, KeyCode.D);
            CommonDrunkennessEffects();
        }

        public override void StopDrinkingEffect()
        {
            Indestructibles.Controls = 
                new KeyControls(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A);
        }
    }
}