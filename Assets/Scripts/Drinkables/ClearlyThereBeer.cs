namespace Drinkables
{
    public class ClearlyThereBeer : AbstractDrinkable
    {
        private void Awake()
        {
            Drink();
            Invoke($"StopDrinkingEffect", 10.0f);
            Invoke($"SoberUp", Indestructibles.SoberingTime);
        }

        protected override void Drink()
        {
            Indestructibles.PlayerData.ClearlyThereStack++;

            foreach (var renderComponent in Indestructibles.Renderers)
            {
                renderComponent.enabled = false;
            }

            Indestructibles.PlayerData.ScoreMultiplier /= 4;
            Indestructibles.PlayerData.LastSeenPosition = Indestructibles.Player.transform.position;
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
                    renderComponent.enabled = true;
                }
            }

            Indestructibles.PlayerData.ClearlyThereStack--;
            Indestructibles.PlayerData.ScoreMultiplier *= 4;
        }
    }
}