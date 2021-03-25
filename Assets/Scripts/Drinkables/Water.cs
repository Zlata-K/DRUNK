namespace Drinkables
{
    public class Water : AbstractDrinkable
    {
        private void Awake()
        {
            Drink();
        }
        
        protected override void Drink()
        {
            SoberUp();
        }

        protected override void StopDrinkingEffect()
        {
            // does nothing
        }
    }
}