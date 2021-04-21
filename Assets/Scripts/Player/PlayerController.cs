using System;
using Drinkables;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] gruntSounds;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private bool debugMode;
        private static readonly int IntoxicationHash = Animator.StringToHash("Intoxication");

        private bool _invincible;
        private bool _onHitSoberUp = true;
        private int _healthPoints = 3;
        private bool _isDead;
        
        private GameObject _belly;
        private Rigidbody _rigidbody;
        private SkinnedMeshRenderer[] _renderers;
        private AudioSource _audioSource;
        private Animator _animator;
        private NPCManager _npcManager;
        
        private Vignette _vignette;
        
        private void Awake()
        {
            _belly = GameObject.Find("Belly");
            
            _rigidbody = GetComponent<Rigidbody>();
            _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            
            Indestructibles.Volume.profile.TryGetSettings(out _vignette);
        }

        private void Update()
        {
            if (debugMode)
            {
                // Invincible toggle
                if (Input.GetKeyDown(KeyCode.I))
                {
                    _invincible = !_invincible;
                } 
                // OnHit Sober Up toggle
                if (Input.GetKeyDown(KeyCode.O))
                {
                    _onHitSoberUp = !_onHitSoberUp;
                } 
                // drink a regular beer every time the player presses U
                if (Input.GetKeyDown(KeyCode.U))
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
            _invincible = false;
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

            if (other.gameObject.CompareTag("Hand") && !_invincible)
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
                        Indestructibles.PlayerData.IntoxicationLevel = 0.0f;
                        _animator.SetTrigger(Animator.StringToHash("Knocked Out"));
                        gameManager.OnPlayerDeath();
                        ClearEffects();
                        return;
                    }
                    
                    // Player got hit but is not dead
                    var direction = (transform.position - npcManager.transform.position ).normalized;
                    
                    // Temporarily disable root motion to add a push force
                    Indestructibles.PlayerAnimator.applyRootMotion = false;
                    _rigidbody.AddForce(direction * 5.0f, ForceMode.Impulse);
                    Invoke(nameof(EnableRootMotion), 0.5f);

                    _invincible = true;
                    Invoke(nameof(DisableInvincible),2.0f);
                
                    // Flicker effect
                    InvokeRepeating(nameof(FlickerModel),0.0f,0.125f);
                    Invoke(nameof(StopFlicker),2.0f);
                    
                    // Sober up
                    if (_onHitSoberUp)
                    {
                        Indestructibles.PlayerData.IntoxicationLevel -= 0.5f;
                        if (Indestructibles.PlayerData.IntoxicationLevel < 0.0f)
                            Indestructibles.PlayerData.IntoxicationLevel = 0.0f;
                        _animator.SetFloat(IntoxicationHash, Indestructibles.PlayerData.IntoxicationLevel);
                        if (_vignette != null)
                        {
                            _vignette.intensity.value = Indestructibles.PlayerData.IntoxicationLevel;
                        }
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