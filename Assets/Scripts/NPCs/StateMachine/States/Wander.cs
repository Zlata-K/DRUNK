using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : State
{
    
    private int wander_distance = 5;
    private int radius = 20;
    private Vector3 target;
    private bool reachedTarget = true;
    
    public Wander()
    {
        name = "Wander";
    }

    public override void Move(GameObject player, GameObject npc)
    {
        WanderForce(player,  npc);
    }
    
    private void WanderForce(GameObject player, GameObject npc)
    {
        if (reachedTarget)
        {
            Vector3 circle_position = npc.transform.position +
                              Vector3.Normalize(npc.GetComponent<Rigidbody>().velocity) * wander_distance;
            float random_angle = Random.Range(0.0f, 360.0f);

             target = circle_position + Random.insideUnitSphere * radius;
            target.y = 0;
            reachedTarget = false;
        }
        
        Vector3 direction = Vector3.Normalize(npc.transform.position - target);
        
        Vector3 velocity = direction ;
        
        npc.GetComponent<Animator>().SetFloat(Animator.StringToHash("Velocity X"), velocity.x);
        npc.GetComponent<Animator>().SetFloat(Animator.StringToHash("Velocity Z"), velocity.z);

        if (Vector3.Distance(npc.transform.position, target) < 1)
        {
            reachedTarget = true;
        }
        
        LookWhereYouAreGoing(npc, velocity);

    }
    
    private void LookWhereYouAreGoing(GameObject npc, Vector3 direction)
    {
        var lookRotation = Quaternion.LookRotation(-1 * direction);
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, lookRotation, 100f * Time.deltaTime);

        Quaternion.Angle(npc.transform.rotation, lookRotation);
    }
}
