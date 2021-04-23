using System;
using Drinkables;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] gruntSounds;
        [SerializeField] private GameManager gameManager;

        [Header("Debug Variables")]
        [SerializeField] private bool invincible;
        [SerializeField] private bool onHitSoberUp;
        
        private int _healthPoints = 3;
        private bool _isDead;
      
        private Rigidbody _rigidbody;
        private SkinnedMeshRenderer[] _renderers;
        private AudioSource _audioSource;
        private Animator _animator;
        private NPCManager _npcManager;
        
        private Vignette _vignette;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            
            Indestructibles.Volume.profile.TryGetSettings(out _vignette);
        }

        private void Update()
        {
            if (Indestructibles.DebugEnabled)
            {
                // Invincible toggle
                if (Input.GetKeyDown(Indestructibles.DebugControls.ToggleInvincible))
                {
                    invincible = !invincible;
                } 
                // OnHit Sober Up toggle
                if (Input.GetKeyDown(Indestructibles.DebugControls.ToggleSoberOnHit))
                {
                    onHitSoberUp = !onHitSoberUp;
                } 
                // drink a regular beer every time the player presses U
                if (Input.GetKeyDown(Indestructibles.DebugControls.DrinkABeer))
                {
                    var beer = new GameObject();
                    beer.transform.parent = transform;
                    beer.AddComponent<RegularBeer>();
                } 
            }
        }

        void EnableRootMotion()
        {
            Indestructibles.PlayerAnimator.applyRootMotion = true;
        }
        void DisableInvincible()
        {
            invincible = false;
        }

        void FlickerModel()
        {
            foreach (var rend in _renderers)
            {
                rend.enabled = !rend.enabled;
            }
        }

        void StopFlicker()
        {
            foreach (var rend in _renderers)
            {
                rend.enabled = true;
            }
            CancelInvoke(nameof(FlickerModel));
        }

        void ClearEffects()
        {
            Indestructibles.Volume.profile.TryGetSettings(out _vignette);
            _animator.SetFloat(Animator.StringToHash("Intoxication"), Indestructibles.PlayerData.IntoxicationLevel);
            if (_vignette != null)
            {
                _vignette.intensity.value = Indestructibles.PlayerData.IntoxicationLevel;
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("NPC"))
            {
                var npcManager = other.gameObject.GetComponent<NPCManager>();
                if (npcManager.IsChasing())
                {
                    npcManager.StartPunching();  
                }
            }

            if (other.gameObject.CompareTag("Hand") && !invincible)
            {
                var npcManager = other.transform.root.gameObject.GetComponent<NPCManager>();
                if (npcManager.IsChasing() && npcManager.Punching && !Indestructibles.PlayerData.IsKnockedOut)
                {
                    npcManager.OnPlayerHit();
                    // Play a random grunt sound
                    if (gruntSounds.Length > 0)
                    {
                        _audioSource.PlayOneShot(gruntSounds[Random.Range(0,gruntSounds.Length-1)]);
                    }
                    
                    // Player is dead
                    if (--_healthPoints <= 0)
                    {
                        Indestructibles.PlayerData.IsKnockedOut = true;
                        _animator.SetTrigger(Animator.StringToHash("Knocked Out"));
                        gameManager.OnPlayerDeath();
                        Indestructibles.PlayerData.IntoxicationLevel = 0.0f;
                        ClearEffects();
                        return;
                    }
                    
                    // Player got hit but is not dead
                    var direction = (transform.position - npcManager.transform.position ).normalized;
                    
                    // Temporarily disable root motion to add a push force
                    Indestructibles.PlayerAnimator.applyRootMotion = false;
                    _rigidbody.AddForce(direction * 5.0f, ForceMode.Impulse);
                    Invoke(nameof(EnableRootMotion), 0.5f);

                    invincible = true;
                    Invoke(nameof(DisableInvincible),2.0f);
                
                    // Flicker effect
                    InvokeRepeating(nameof(FlickerModel),0.0f,0.125f);
                    Invoke(nameof(StopFlicker),2.0f);
                    
                    // Sober up
                    if (onHitSoberUp)
                    {
                        Indestructibles.PlayerData.IntoxicationLevel -= 0.25f;
                        ClearEffects();
                    }
                }
                
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("NPC"))
            {
                var npcManager = other.gameObject.GetComponent<NPCManager>();
                if (npcManager.IsChasing())
                {
                    npcManager.StartPunching();  
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("NPC"))
            {
                other.gameObject.GetComponent<NPCManager>().StopPunching();
                
            }
        }

    }
}