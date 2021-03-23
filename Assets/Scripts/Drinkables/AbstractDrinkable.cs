using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Drinkables
{
    /**
     * The drinkable should take care of all of the logic pertaining to consuming a drinkable.
     * Once drink is called by the player, the logic is delegated to the drinkable.
     * The drinkable will start the effect of whatever the player drank and STOP the effect.
     */
    public abstract class AbstractDrinkable
    {
        private Vignette _vignette;

        protected float EffectTimer; // timer to stop the beer's special effect (independent of sobering)
        private float _drunkennessTimer = Indestructibles.SoberingTime;

        public abstract void Drink();

        public abstract void StopDrinkingEffect();

        public float GetDrunkennessTimer()
        {
            return _drunkennessTimer;
        }

        public void SetBeerTimeLeftInBody(float timeSinceLastCheck)
        {
            _drunkennessTimer -= timeSinceLastCheck;
        }

        public float GetEffectTimer()
        {
            return EffectTimer;
        }

        public void SetEffectTimeLeft(float timeSinceLastCheck)
        {
            EffectTimer -= timeSinceLastCheck;
        }
        
        protected void CommonDrunkennessEffects()
        {
            // All beers will have a base score multiplier of 2
            Indestructibles.ScoreMultiplier *= 2;
            
            // the effects can be changed, i just put vignette first as a PoC
            Indestructibles.Volume.profile.TryGetSettings(out _vignette);
            if (_vignette != null)
            {
                //_vignette.intensity.value = Mathf.Min(_vignette.intensity.value + 0.1f, 1);
                _vignette.intensity.value += 0.1f;
            }
        }

        public void SoberUp()
        {
            Indestructibles.ScoreMultiplier /= 2;
            if (_vignette != null)
            {
                // _vignette.intensity.value = Mathf.Max(_vignette.intensity.value - 0.1f, 0);
                _vignette.intensity.value -= 0.1f;
            }
        }
    }
}