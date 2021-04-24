using UnityEngine;

namespace Drinkables
{
    public class BlueBuffaloBeer : AbstractDrinkable
    {

        private void Awake()
        {
            Drink();
            Invoke($"StopDrinkingEffect", 10.0f);
            Invoke($"SoberUp", Indestructibles.SoberingTime);
            Indestructibles.UIManager.AddEffect("BlueBuffalo", Color.blue, 10.0f);
        }
        
        protected override void Drink()
        {
            Indestructibles.Player.GetComponent<Animator>().speed = 1.5f;
            Indestructibles.PlayerData.CurrentScore += 10 * Indestructibles.PlayerData.ScoreMultiplier;
            CommonDrunkennessEffects();
        }

        protected override void StopDrinkingEffect()
        {
            Indestructibles.Player.GetComponent<Animator>().speed = 1.2f;
        }
    }
}