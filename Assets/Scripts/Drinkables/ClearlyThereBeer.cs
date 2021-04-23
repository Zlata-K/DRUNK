using UnityEngine;

namespace Drinkables
{
    public class ClearlyThereBeer : AbstractDrinkable
    {
        private void Awake()
        {
            Drink();
            Invoke($"StopDrinkingEffect", 10.0f);
            Invoke($"SoberUp", Indestructibles.SoberingTime);
            Indestructibles.UIManager.AddEffect("ClearlyThere", Color.yellow, 15.0f);
        }

        protected override void Drink()
        {
            Indestructibles.PlayerData.ClearlyThereStack++;
            
            if (Indestructibles.PlayerData.ClearlyThereStack == 1)
            {
                foreach (var renderComponent in Indestructibles.Renderers)
                {
                    Material material;
                    (material = renderComponent.material).shader = Shader.Find("Transparent/Diffuse");

                    // I have no choice to make a transparency variable and assign it
                    // Since renderComponent.material.color.a returns a temporary value
                    var transparency = material.color;
                    transparency.a = 0.2f;
                    material.color = transparency;
                }
            }
            
            Indestructibles.PlayerData.LastSeenPosition = Indestructibles.Player.transform.position;
            Indestructibles.PlayerData.CurrentScore += 10 * Indestructibles.PlayerData.ScoreMultiplier;
            CommonDrunkennessEffects();
        }

        protected override void StopDrinkingEffect()
        {
            // Only go back to being seen if there are no other `Clearly There` beers
            // in the player's belly
            if (Indestructibles.PlayerData.ClearlyThereStack == 1)
            {
                foreach (var renderComponent in Indestructibles.Renderers)
                {
                    Material material;
                    (material = renderComponent.material).shader = Shader.Find("Standard");

                    var transparency = material.color;
                    transparency.a = 1.0f;
                    material.color = transparency;
                }
            }

            Indestructibles.PlayerData.ClearlyThereStack--;
        }
    }
}