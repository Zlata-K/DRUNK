using System;
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
        private static readonly int IntoxicationHash = Animator.StringToHash("Intoxication");
        protected abstract void Drink();

        protected abstract void StopDrinkingEffect();

        protected void CommonDrunkennessEffects()
        {
            Indestructibles.PlayerData.IntoxicationLevel += 0.1f;
           
            Indestructibles.PlayerAnimator.SetFloat(IntoxicationHash, Indestructibles.PlayerData.IntoxicationLevel);
            // the effects can be changed, i just put vignette first as a PoC
            Indestructibles.Volume.profile.TryGetSettings(out _vignette);
            if (_vignette != null)
            {
                _vignette.intensity.value = Indestructibles.PlayerData.IntoxicationLevel;
            }
            Indestructibles.PlayerData.ScoreMultiplier = Mathf.CeilToInt(Mathf.Exp(4.0f * Indestructibles.PlayerData.IntoxicationLevel));
            Indestructibles.UIManager.RefreshUI();
        }

        protected void SoberUp()
        {
            Indestructibles.PlayerData.IntoxicationLevel -= 0.1f;
            Indestructibles.PlayerAnimator.SetFloat(IntoxicationHash, Indestructibles.PlayerData.IntoxicationLevel);
            if (_vignette != null)
            {
                _vignette.intensity.value = Indestructibles.PlayerData.IntoxicationLevel;
            }
            Indestructibles.PlayerData.ScoreMultiplier = (int) Mathf.Exp(4.0f * Indestructibles.PlayerData.IntoxicationLevel);
            Indestructibles.UIManager.RefreshUI();
            Destroy(gameObject);
        }
    }
}