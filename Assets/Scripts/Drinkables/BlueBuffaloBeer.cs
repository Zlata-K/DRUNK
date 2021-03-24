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
        }
        
        protected override void Drink()
        {
            Indestructibles.Player.GetComponent<Animator>().speed += 0.1f;
            Indestructibles.PlayerData.ScoreMultiplier /= 4;
        }

        protected override void StopDrinkingEffect()
        {
            Indestructibles.Player.GetComponent<Animator>().speed -= 0.1f;
            Indestructibles.PlayerData.ScoreMultiplier *= 4;
        }
    }
}