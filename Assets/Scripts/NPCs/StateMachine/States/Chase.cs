using Player;
using UnityEngine;

public class Chase : State
{
    public Chase()
    {
        name = "Chase";
    }

    public override void Move(GameObject player, GameObject npc)
    {
        Chasing(player, npc);
    }
    
    private void Chasing(GameObject player, GameObject npc)
    {
        Vector3 futureLocation = player.transform.position + player.GetComponent<PlayerDataManager>().PlayerData.Velocity * NPCsGlobalVariables.ChasePredictionMultiplier;
        
        Vector3 desiredVelocity = Vector3.Normalize(futureLocation - npc.transform.position) * NPCsGlobalVariables.ChaseAcceleration;
        
        Vector3 currentVelocity = npc.GetComponent<NPCManager>().Velocity;

        Vector3 steering = desiredVelocity - currentVelocity;

        Vector3 velocity = currentVelocity + steering;

        if (Vector3.Magnitude(velocity) > NPCsGlobalVariables.ChaseMaxVelocity)
        {
            velocity = (velocity / Vector3.Magnitude(velocity)) * NPCsGlobalVariables.ChaseMaxVelocity;
        }

        velocity.y = 0;

        //The NPC will always looking where it needs to go.
        LookWhereYouAreGoing(npc, velocity);

        //The NPC always walk forward.
        velocity = Vector3.forward * velocity.magnitude;

        npc.GetComponent<Animator>().SetFloat(NPCsGlobalVariables.VelocityXHash, velocity.x);
        npc.GetComponent<Animator>().SetFloat(NPCsGlobalVariables.VelocityZHash, velocity.z);
    }
}
