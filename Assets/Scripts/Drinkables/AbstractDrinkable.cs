using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Drinkables
{
    /**
     * The drinkable should take care of all of the logic pertaining to consuming a drinkable.
     * Once drink is called by the player, the logic is delegated to the drinkable.
     * The drinkable will start the effect of whatever the player drank and STOP the effect.
     */
    public abstract class AbstractDrinkable : MonoBehaviour
    {
        private Vignette _vignette;

        protected abstract void Drink();

        protected abstract void StopDrinkingEffect();

        protected void CommonDrunkennessEffects()
        {
            // All beers will have a base score multiplier of 2
            Indestructibles.PlayerData.ScoreMultiplier *= 2;

            // the effects can be changed, i just put vignette first as a PoC
            Indestructibles.Volume.profile.TryGetSettings(out _vignette);
            if (_vignette != null)
            {
                _vignette.intensity.value += 0.1f;
            }
        }

        protected void SoberUp()
        {
            Indestructibles.PlayerData.ScoreMultiplier /= 2;
            if (_vignette != null)
            {
                _vignette.intensity.value -= 0.1f;
            }

            Destroy(gameObject);
        }
    }
}