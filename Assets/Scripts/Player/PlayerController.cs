using Drinkables;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        private bool _invincible;
        
        
        private GameObject _belly;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _belly = GameObject.Find("Belly");
            _rigidbody = GetComponent<Rigidbody>();
            
            // drink a regular beer every time the player presses U
            if (Input.GetKeyDown(KeyCode.U))
            {
                var beer = new GameObject();
                beer.transform.parent = transform;
                beer.AddComponent<RegularBeer>();
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
        
        private void OnCollisionEnter(Collision other)
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
                if (npcManager.IsChasing())
                {
                    var direction = (transform.position - npcManager.transform.position ).normalized;
                    // Temporarily disable root motion to add a push force
                    Indestructibles.PlayerAnimator.applyRootMotion = false;
                    _rigidbody.AddForce(direction * 5.0f, ForceMode.Impulse);
                    Invoke(nameof(EnableRootMotion), 0.5f);

                    _invincible = true;
                    Invoke(nameof(DisableInvincible),2.0f);
                    
                    npcManager.OnPlayerHit();
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