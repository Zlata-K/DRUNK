using System;
using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking.Behaviour
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Chase")]

//Make the agent move towards the players
    public class ChaseBehaviour : FilteredFlockBehavior
    {
        public override Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock)
        {
            Vector3 desiredVelocity = Vector3.Normalize(flock.CurrentTargetLocation - npc.transform.position) *
                                      npc.GetModelSpeed(NpcGlobalVariables.ChaseMaxVelocity);

            Vector3 currentVelocity = npc.Rigidbody.velocity;

            Vector3 steering = desiredVelocity - currentVelocity;

            if (steering.magnitude > NpcGlobalVariables.ChaseAcceleration)
            {
                steering = (steering / Vector3.Magnitude(steering)) *
                           npc.GetModelSpeed(NpcGlobalVariables.ChaseAcceleration);
            }
            
            Vector3 velocity = currentVelocity + steering;

            if (Vector3.Magnitude(velocity) > npc.GetModelSpeed(NpcGlobalVariables.ChaseMaxVelocity))
            {
                velocity = (velocity / Vector3.Magnitude(velocity)) *
                           npc.GetModelSpeed(NpcGlobalVariables.ChaseMaxVelocity);
            }

            velocity.y = 0;

            CheckNPCReachedTarget(npc, flock);

            return velocity;
        }

        private void CheckNPCReachedTarget(NPCManager npc, FlockManager flock)
        {
            Vector3 targetWithoutY = new Vector3(flock.CurrentTargetLocation.x, 0, flock.CurrentTargetLocation.z);

            if (Vector3.Distance(targetWithoutY, npc.transform.position) <
                1.5f)
            {
                flock.AgentReachedTarget(npc);
            }
        }
    }
}