using UnityEngine;

namespace Drinkables
{
    public class RegularBeer : AbstractDrinkable
    {
        public override void Drink()
        {
            // beers with special effects would have the logic here
            CommonDrunkennessEffects(); // not called by water (water will sober the player)
        }

        public override void StopDrinkingEffect()
        {
            // beers with special effects would have the undo logic here
            CommonSoberingEffects(); // not called by water
        }
    }
}
