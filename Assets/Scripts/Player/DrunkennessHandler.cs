using System.Collections.Generic;
using System.Linq;
using Drinkables;

namespace Player
{
    public static class DrunkennessHandler
    {
        public static void OnDrink(AbstractDrinkable chosenDrink)
        {
            chosenDrink.Drink();
        }

        public static void OnSoberingUp(IEnumerable<AbstractDrinkable> drinksInBody, float timeSinceLastCheck)
        {
            var abstractDrinkables = drinksInBody.ToList();
            
            for (var i = abstractDrinkables.Count - 1; i >= 0; i--)
            {
                if (abstractDrinkables[i].GetDrunkennessTimer() <= 0.0f)
                {
                    abstractDrinkables[i].StopDrinkingEffect();
                    abstractDrinkables.RemoveAt(i);
                }
                else
                {
                    abstractDrinkables[i].SetDrunkennessTimer(timeSinceLastCheck);
                }

                // this is to avoid bugs if the player drinks while the sobering function is being called
                if (i == 0)
                {
                    break;
                }
            }
        }
    }
}
