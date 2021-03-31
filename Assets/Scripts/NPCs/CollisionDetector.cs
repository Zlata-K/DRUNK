using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    /*
     * The rigidbody cannot be on the same object as the animator when the animator uses root motion.
     * It causes the game object to flicker.
     * 
     * This MonoBehaviour checks for collisions and alert the stateMachine.
     */
    private NPCStateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = GetComponentInParent<NPCStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _stateMachine.CollidedWithPlayer();
        }
    }
}
