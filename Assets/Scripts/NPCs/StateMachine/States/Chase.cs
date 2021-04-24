using System;
using System.Collections.Generic;
using Player;
using Structs;
using UnityEngine;

public class Chase : State
{
    private readonly PlayerData _playerData;
    private readonly FlockBehaviour _avoidObstacles;
    private readonly FlockBehaviour _avoidNPCs;
    private Vector3 _currentTargetLocation = NPCsGlobalVariables.DefaultInitialVector;
    private readonly LayerMask _environmentMask;

    public Chase(NPCManager npcManager, FlockBehaviour avoidObstacles, FlockBehaviour avoidNPCs)
    {
        _npcManager = npcManager;
        _playerData = Indestructibles.Player.GetComponent<PlayerDataManager>().PlayerData;
        _environmentMask = LayerMask.GetMask("Environment");
        _avoidObstacles = avoidObstacles;
        _avoidNPCs = avoidNPCs;
    }

    public override void Move()
    {
        Chasing();
    }

    private void Chasing()
    {
        Vector3 velocity = ComputeChaseVelocity();
        
        //The NPC will always looking where it needs to go.
        float anglePlayerNpc = _npcManager.LookWhereYouAreGoing(velocity);

        //If the player is out FOV, NPC stop moving and rotate until the player is back in FOV
        if (anglePlayerNpc > NPCsGlobalVariables.FieldOfView && _npcManager.GetDistanceWithPlayer() > NPCsGlobalVariables.ChasePlayerRange)
        {
            velocity = Vector3.zero;
        }

        //The NPC always walk forward.
        _npcManager.SetAnimatorVelocity(Vector3.forward * velocity.magnitude);
    }

    private Vector3 ComputeChaseVelocity()
    {
        Ray ray = new Ray(_npcManager.transform.position, _playerData.LastSeenPosition);
        float maxDistance = Mathf.Min(NPCsGlobalVariables.ChasePlayerRange,
            Vector3.Distance(_npcManager.transform.position, _playerData.LastSeenPosition));
        if (!Physics.SphereCast(ray, 0.5f, maxDistance, _environmentMask))
        {
            _currentTargetLocation = _playerData.LastSeenPosition;
        }
        else
        {
            try
            {
                ComputeAStarForSteering();
            }
            catch (Exception e)
            {
                _currentTargetLocation = _playerData.LastSeenPosition;
            }
        }

        // TODO Check if we can't see the target (raycast) and do smth
        Vector3 desiredVelocity = Vector3.Normalize(_currentTargetLocation - _npcManager.transform.position) *
                                  _npcManager.GetModelSpeed(NPCsGlobalVariables.ChaseAcceleration);

        Vector3 currentVelocity = _npcManager.Rigidbody.velocity;

        Vector3 steering = desiredVelocity - currentVelocity;

        Vector3 velocity = currentVelocity + steering;
        
        velocity += _avoidObstacles.CalculateMove(_npcManager, _npcManager.GetNearbyObjects(1f), null) * 1.5f;
        velocity += _avoidNPCs.CalculateMove(_npcManager, _npcManager.GetNearbyObjects(1f), null);

        if (Vector3.Magnitude(velocity) > _npcManager.GetModelSpeed(NPCsGlobalVariables.ChaseMaxVelocity))
        {
            velocity = (velocity / Vector3.Magnitude(velocity)) *
                       _npcManager.GetModelSpeed(NPCsGlobalVariables.ChaseMaxVelocity);
        }

        velocity.y = 0;

        return velocity;
    }
    
    private void ComputeAStarForSteering()
    {
        Node npcNode = NavigationGraph.GetClosestNode(_npcManager.transform.position);
        Node playerNode = NavigationGraph.GetClosestNode(_playerData.LastSeenPosition);

        List<Node> path = Pathfinding.Astar(npcNode, playerNode);

        if (path == null)
        {
            Debug.Log(npcNode);
        }
        
        _currentTargetLocation = path[NPCsGlobalVariables.NextElementInPath].position;
    }
}