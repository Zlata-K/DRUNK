using System.Collections.Generic;
using Player;
using Structs;
using UnityEngine;

public class Chase : State
{
    private Vector3 _currentTargetLocation = NPCsGlobalVariables.DefaultInitialVector;
    private readonly PlayerData _playerData;

    public Chase(NPCManager npcManager)
    {
        _npcManager = npcManager;
        _playerData = Indestructibles.Player.GetComponent<PlayerDataManager>().PlayerData;
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
        Vector3 targetLocation;

        //The NPC will predict the future position of the player (pursuit behavior)
        if (_npcManager.GetDistanceWithPlayer() > NPCsGlobalVariables.ChasePlayerRange) // TODO add raycast here
        {
            ComputeAStar();
            targetLocation = _currentTargetLocation;
        }
        //If the NPC is close to the player, just go directly on him
        else
        {
            _currentTargetLocation = NPCsGlobalVariables.DefaultInitialVector;
            targetLocation = _playerData.LastSeenPosition;
        }

        // TODO Check if we can't see the target (raycast) and do smth
        Vector3 desiredVelocity = Vector3.Normalize(targetLocation - _npcManager.transform.position) *
                                  _npcManager.GetModelSpeed(NPCsGlobalVariables.ChaseAcceleration);

        Vector3 currentVelocity = _npcManager.Rigidbody.velocity;

        Vector3 steering = desiredVelocity - currentVelocity;

        Vector3 velocity = currentVelocity + steering;

        if (Vector3.Magnitude(velocity) > _npcManager.GetModelSpeed(NPCsGlobalVariables.ChaseMaxVelocity))
        {
            velocity = (velocity / Vector3.Magnitude(velocity)) *
                       _npcManager.GetModelSpeed(NPCsGlobalVariables.ChaseMaxVelocity);
        }

        velocity.y = 0;

        return velocity;
    }

    private void ComputeAStar()
    {
        Vector3 nodeWithoutY = Vector3.zero;
        if (_currentTargetLocation != NPCsGlobalVariables.DefaultInitialVector)
        {
            nodeWithoutY = new Vector3(_currentTargetLocation.x, 0, _currentTargetLocation.z);
        }

        if (_currentTargetLocation == NPCsGlobalVariables.DefaultInitialVector ||
            Vector3.Distance(_npcManager.transform.position, nodeWithoutY) <
            NPCsGlobalVariables.WithinTargetNodeRange) // TODO Add check if we can see target node
        {
            Node npcNode = NavigationGraph.GetClosestNode(_npcManager.transform.position);
            Node playerNode = NavigationGraph.GetClosestNode(_playerData.LastSeenPosition);

            List<Node> path = Pathfinding.Astar(npcNode, playerNode);

            if (path.Count < 2)
            {
                _currentTargetLocation = _playerData.LastSeenPosition;
            } else
            {
                _currentTargetLocation = path[NPCsGlobalVariables.NextElementInPath].position;
            }
        }
    }
}