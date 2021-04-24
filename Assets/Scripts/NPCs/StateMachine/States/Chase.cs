using System.Collections.Generic;
using Player;
using Structs;
using UnityEngine;

public class Chase : State
{
    private readonly PlayerData _playerData;
    private readonly FlockBehaviour _avoidObstacles;
    private readonly FlockBehaviour _avoidNPCs;

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
        Vector3  targetLocation = _playerData.LastSeenPosition;

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
}