using UnityEngine;

namespace Structs
{
    public struct DebugControls
    {
        public KeyCode DrinkBeer { get; set; }
        public KeyCode DrinkClearlyThere { get; set; }
        public KeyCode DrinkBuffalo { get; set; }
        public KeyCode DrinkUpsideDown { get; set; }
        public KeyCode ToggleInvincible { get; set; }
        public KeyCode ToggleSoberOnHit { get; set; }

        public DebugControls(
            KeyCode drinkBeer,
            KeyCode drinkClearlyThere,
            KeyCode drinkBuffalo,
            KeyCode drinkUpsideDown,
            KeyCode toggleInvincible, 
            KeyCode toggleSoberOnHit)
        {
            DrinkBeer = drinkBeer;
            DrinkClearlyThere = drinkClearlyThere;
            DrinkBuffalo = drinkBuffalo;
            DrinkUpsideDown = drinkUpsideDown;
            ToggleInvincible = toggleInvincible;
            ToggleSoberOnHit = toggleSoberOnHit;
        }
    }
}