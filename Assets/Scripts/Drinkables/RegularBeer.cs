namespace Drinkables
{
    public class RegularBeer : AbstractDrinkable
    {
        public override void Drink()
        {
            CommonDrunkennessEffects();
        }

        public override void StopDrinkingEffect()
        {
            // no effect
        }
    }
}
