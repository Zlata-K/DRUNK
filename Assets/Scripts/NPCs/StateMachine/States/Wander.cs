using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : State
{
    private Vector3 _target;
    private bool reachedTarget = true;
    
    /*
     * After a random number of moves (between 1 and 10), the NPC stop moving for few seconds.
     */
    private int numberOfMoves = 0;
    private float waitTimeStamp = 0;
    
    public Wander()
    {
        name = "Wander";
        numberOfMoves = (int)Random.Range(1.0f, 10.0f);
    }

    public override void Move(GameObject player, GameObject npc)
    {
        if (waitTimeStamp < Time.time)
        {
            Wandering(npc);
        }
    }
    
    private void Wandering(GameObject npc)
    {
        if (reachedTarget)
        {
           FindNewTarget(npc);
        }
        
        MoveTowardsTarget(npc);

        if (Vector3.Distance(npc.transform.position, _target) < 1.0f)
        {
            reachedTarget = true;
            numberOfMoves -= 1;
        }

        if (numberOfMoves == 0)
        {
            StopMoving(npc);
        }
    }

    private void FindNewTarget(GameObject npc)
    {
        Vector3 npcVelocity = npc.GetComponent<NPCManager>().Velocity;
            
        Vector3 circle_position = npc.transform.position +
                                  Vector3.Normalize(npcVelocity) * NPCsGlobalVariables.WanderCircleDistance;

        _target = circle_position + Random.insideUnitSphere * NPCsGlobalVariables.WanderCircleRadius;
        _target.y = 0;
            
        reachedTarget = false;
    }

    private void MoveTowardsTarget(GameObject npc)
    {
        Vector3 direction = Vector3.Normalize(_target - npc.transform.position);

        //The NPC will always looking where it needs to go.
        LookWhereYouAreGoing(npc, direction);

        //The NPC always walk forward.
        Vector3 velocity = Vector3.forward * NPCsGlobalVariables.WanderMaxVelocity;

        npc.GetComponent<Animator>().SetFloat(NPCsGlobalVariables.VelocityXHash, velocity.x);
        npc.GetComponent<Animator>().SetFloat(NPCsGlobalVariables.VelocityZHash, velocity.z);
    }

    private void StopMoving(GameObject npc)
    {
        waitTimeStamp = NPCsGlobalVariables.WanderWaitTime + Time.time;
        numberOfMoves = (int)Random.Range(0.0f, 10.0f);
        npc.GetComponent<Animator>().SetFloat(NPCsGlobalVariables.VelocityXHash, 0);
        npc.GetComponent<Animator>().SetFloat(NPCsGlobalVariables.VelocityZHash, 0);
    }
}
