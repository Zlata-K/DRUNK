using System;
using UnityEditor.Animations;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{

    public State CurrentState { get; set;}

    private State _wander;
    private State _idle;
    private State _chase;

    private NPCManager _npcManager;
    
    void Awake()
    {
        _npcManager = GetComponent<NPCManager>();

        _wander = new Wander(_npcManager);
        _chase = new Chase(_npcManager);
        _idle = new Idle(_npcManager);

        CurrentState = _wander;
    }

    private void Update()
    {
        CurrentState.Move();
        float distanceFromPlayer = Vector3.Distance( Indestructibles.Player.transform.position, transform.position);
        CheckPlayerOutOfRange(distanceFromPlayer);
        CheckPlayerBackInRange(distanceFromPlayer);
    }

    /*
     * If the player get out of range during chase, start wandering.
     */
    private void CheckPlayerOutOfRange(float distanceFromPlayer)
    {
        if ( CurrentState == _chase &&
             distanceFromPlayer > NPCsGlobalVariables.MaxChaseDistance)
        {
            CurrentState = _wander;
            _npcManager.LookingForPlayer = true;
        }
    }

    /*
     * If the player is back in range and was previously chased by the NPC, chase again.
     */
    private void CheckPlayerBackInRange(float distanceFromPlayer)
    {
        if (CurrentState == _wander &&
                 _npcManager.LookingForPlayer &&
                 distanceFromPlayer < NPCsGlobalVariables.MaxChaseDistance)
        {
            CurrentState = _chase;
        }
    }

    public void StartWandering()
    {
        CurrentState = _wander;
    }

    public void StartChasing()
    {
        CurrentState = _chase;
    }
}
