using UnityEngine;

namespace Structs
{
    public struct DebugControls
    {
        public KeyCode DrinkABeer { get; set; }
        public KeyCode ToggleInvincible { get; set; }
        public KeyCode ToggleSoberOnHit { get; set; }

        public DebugControls(KeyCode drinkABeer, KeyCode toggleInvincible, KeyCode toggleSoberOnHit)
        {
            DrinkABeer = drinkABeer;
            ToggleInvincible = toggleInvincible;
            ToggleSoberOnHit = toggleSoberOnHit;
        }
    }
}