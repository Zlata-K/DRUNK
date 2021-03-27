using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chase : State
{
    private const int _predictionValue = 3;
    private const float _maxVelocity = 5f;

    public Chase()
    {
        name = "Chase";
    }

    public override void Move(GameObject player, GameObject npc)
    {
        Seek(player, npc);
    }

    private void Pursuit(GameObject player)
    {

        
    }
    
    private void Seek(GameObject player, GameObject npc)
    {
        Vector3 desiredVelocity = Vector3.Normalize(player.transform.position - npc.transform.position) * _maxVelocity;

        Vector3 steering = desiredVelocity - npc.GetComponent<Rigidbody>().velocity;

        Vector3 velocity = npc.GetComponent<Rigidbody>().velocity + steering;

        if (Vector3.Magnitude(velocity) > _maxVelocity)
        {
            velocity = (velocity / Vector3.Magnitude(velocity)) * _maxVelocity;
        }

        velocity.y = 0;
        
        LookWhereYouAreGoing(npc, velocity);
        
        npc.GetComponent<Animator>().SetFloat(Animator.StringToHash("Velocity X"), velocity.x);
        npc.GetComponent<Animator>().SetFloat(Animator.StringToHash("Velocity Z"), velocity.z);
    }

    private void LookWhereYouAreGoing(GameObject npc, Vector3 direction)
    {
        var lookRotation = Quaternion.LookRotation(direction);
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, lookRotation, 100f * Time.deltaTime);

        Quaternion.Angle(npc.transform.rotation, lookRotation);
    }
}
