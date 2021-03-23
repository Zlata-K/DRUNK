namespace Drinkables
{
    public class Water : AbstractDrinkable
    {
        public override void Drink()
        {
            SoberUp();
        }

        public override void StopDrinkingEffect()
        {
            // does nothing
        }
    }
}