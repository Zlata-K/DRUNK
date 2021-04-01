using Player;
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
        Vector3 futureLocation = Indestructibles.Player.transform.position + _npcManager.PlayerRigidbody.velocity * NPCsGlobalVariables.ChasePredictionMultiplier;
        
        Vector3 desiredVelocity = Vector3.Normalize(futureLocation - _npcManager.transform.position) * NPCsGlobalVariables.ChaseAcceleration;
        
        Vector3 currentVelocity = _npcManager.GetComponent<Rigidbody>().velocity;

        Vector3 steering = desiredVelocity - currentVelocity;

        Vector3 velocity = currentVelocity + steering;

        if (Vector3.Magnitude(velocity) > NPCsGlobalVariables.ChaseMaxVelocity)
        {
            velocity = (velocity / Vector3.Magnitude(velocity)) * NPCsGlobalVariables.ChaseMaxVelocity;
        }

        velocity.y = 0;

        //The NPC will always looking where it needs to go.
        LookWhereYouAreGoing(velocity);

        //The NPC always walk forward.
        velocity = Vector3.forward * velocity.magnitude;

        _npcManager.Animator.SetFloat(NPCsGlobalVariables.VelocityXHash, velocity.x);
        _npcManager.Animator.SetFloat(NPCsGlobalVariables.VelocityZHash, velocity.z);
    }
}
