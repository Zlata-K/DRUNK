using System;
using UnityEditor.Animations;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{

    public State CurrentState { get; set;}

    private State _wander;
    private State _idle;
    private State _chase;
    private State _flocking;

    private NPCManager _npcManager;
    private FlockManager _flockManager;

    [SerializeField] private FlockBehaviour _avoidObstacles;
    [SerializeField] private FlockBehaviour _avoidNPCs;

    void Start()
    {
        _npcManager = GetComponent<NPCManager>();
        _flockManager = Indestructibles.FlockManagerInstance;

        _wander = new Wander(_npcManager, _avoidObstacles, _avoidNPCs);
        _chase = new Chase(_npcManager);
        _idle = new Idle();
        _flocking = new Flocking();

        CurrentState = _wander;
    }

    private void Update()
    {
        CurrentState.Move();
        float distanceFromPlayer = Vector3.Distance( Indestructibles.Player.transform.position, transform.position);
        CheckPlayerOutOfRange(distanceFromPlayer);
        CheckPlayerBackInRange(distanceFromPlayer);
        CheckFlockingOrChasing();
        AddToFlock();
    }

    //----- State change functions -----

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
    
    /*
     * If player is in NPC chase range, make the npc chase the player directly
     * Else, make the NPC follow the flock
     */
    private void CheckFlockingOrChasing()
    {
        if (CurrentState == _wander || CurrentState == _idle)
            return;

        if (CurrentState == _flocking &&
            _npcManager.GetDistanceWithPlayer() < NPCsGlobalVariables.ChasePlayerRange)
        {
            CurrentState = _chase;
        }

        if (CurrentState == _chase &&
            _npcManager.GetDistanceWithPlayer() > NPCsGlobalVariables.ChasePlayerRange + 0.2f)
        {
            CurrentState = _flocking;
        }
    }

    private void AddToFlock()
    {
        if (CurrentState == _flocking && !_flockManager.IsNPCInFlock(_npcManager))
        {
            _flockManager.AddNPCToFlock(_npcManager);
        }
        else if ( CurrentState != _flocking && _flockManager.IsNPCInFlock(_npcManager))
        {
            _flockManager.RemoveNPCFromFlock(_npcManager);
        }
    }
}