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
            CommonDrunkennessEffects();
        }

        protected override void StopDrinkingEffect()
        {
            // no effect
        }
    }
}
