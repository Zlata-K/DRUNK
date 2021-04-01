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

    public Wander(NPCManager npcManager)
    {
        _npcManager = npcManager;
        numberOfMoves = (int)Random.Range(1.0f, 10.0f);
    }

    public override void Move()
    {
        if (waitTimeStamp < Time.time)
        {
            Wandering();
        }
    }
    
    private void Wandering()
    {
        if (reachedTarget)
        {
           FindNewTarget();
        }
        
        MoveTowardsTarget();

        if (Vector3.Distance(_npcManager.transform.position, _target) < 1.0f)
        {
            reachedTarget = true;
            numberOfMoves -= 1;
        }

        if (numberOfMoves == 0)
        {
            StopMoving();
        }
    }

    private void FindNewTarget()
    {
        Vector3 npcVelocity = _npcManager.Rigidbody.velocity;
            
        Vector3 circle_position = _npcManager.transform.position +
                                  Vector3.Normalize(npcVelocity) * NPCsGlobalVariables.WanderCircleDistance;

        _target = circle_position + Random.insideUnitSphere * NPCsGlobalVariables.WanderCircleRadius;
        _target.y = 0;
            
        reachedTarget = false;
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = Vector3.Normalize(_target - _npcManager.transform.position);

        //The NPC will always looking where it needs to go.
        LookWhereYouAreGoing(direction);

        //The NPC always walk forward.
        Vector3 velocity = Vector3.forward * NPCsGlobalVariables.WanderMaxVelocity;

        _npcManager.Animator.SetFloat(NPCsGlobalVariables.VelocityXHash, velocity.x);
        _npcManager.Animator.SetFloat(NPCsGlobalVariables.VelocityZHash, velocity.z);
    }

    private void StopMoving()
    {
        waitTimeStamp = NPCsGlobalVariables.WanderWaitTime + Time.time;
        numberOfMoves = (int)Random.Range(0.0f, 10.0f);
        _npcManager.Animator.SetFloat(NPCsGlobalVariables.VelocityXHash, 0);
        _npcManager.Animator.SetFloat(NPCsGlobalVariables.VelocityZHash, 0);
    }
}
