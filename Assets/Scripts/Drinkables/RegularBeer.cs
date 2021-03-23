namespace Drinkables
{
    public class RegularBeer : AbstractDrinkable
    {
        public override void Drink()
        {
            // beers with special effects would have the logic here
            CommonDrunkennessEffects();
        }

        public override void StopDrinkingEffect()
        {
            // beers with special effects would have the undo logic here
            SoberUp(); 
        }
    }
}
