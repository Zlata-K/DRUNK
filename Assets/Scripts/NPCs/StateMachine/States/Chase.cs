using UnityEngine;

public class Chase : State
{
    public Chase(NPCManager npcManager)
    {
        _npcManager = npcManager;
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
        if (anglePlayerNpc > NPCsGlobalVariables.FieldOfView)
        {
            velocity = Vector3.zero;
        }

        //The NPC always walk forward.
        _npcManager.SetAnimatorVelocity(Vector3.forward * velocity.magnitude);
    }

    private Vector3 ComputeChaseVelocity()
    {
        Vector3 goalLocation;
        
        //The NPC will predict the future position of the player (pursuit behavior)
        if (Vector3.Distance(Indestructibles.Player.transform.position, _npcManager.transform.position) > 1)
        {
            goalLocation = Indestructibles.Player.transform.position + _npcManager.PlayerRigidbody.velocity * NPCsGlobalVariables.ChasePredictionMultiplier;
        }
        //If the NPC is close to the player, just go directly on him
        else
        {
            goalLocation = Indestructibles.Player.transform.position;
        }
        
        Vector3 desiredVelocity = Vector3.Normalize(goalLocation - _npcManager.transform.position) * NPCsGlobalVariables.ChaseAcceleration;
        
        Vector3 currentVelocity = _npcManager.Rigidbody.velocity;

        Vector3 steering = desiredVelocity - currentVelocity;

        Vector3 velocity = currentVelocity + steering;

        if (Vector3.Magnitude(velocity) > NPCsGlobalVariables.ChaseMaxVelocity)
        {
            velocity = (velocity / Vector3.Magnitude(velocity)) * NPCsGlobalVariables.ChaseMaxVelocity;
        }

        velocity.y = 0;

        return velocity;
    }
}
