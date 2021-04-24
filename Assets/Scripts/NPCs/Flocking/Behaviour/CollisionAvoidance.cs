using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking.Behaviour
{       
    [CreateAssetMenu(menuName = "Flock/Behaviour/CollisionAvoidance")]
    public class CollisionAvoidance: ScriptableObject
    {
        [SerializeField] PhysicsLayerFilter filter;
        
        public Vector3 CalculateMove(NPCManager npc, Vector3 velocity)
        {
            Vector3 ahead = npc.transform.position + velocity;
            ahead.y = 0;

            Vector3 mostThreatening = findMostThreateningObstacle(ahead, npc);
            Vector3 avoidance = new Vector3(0, 0, 0);

            if (mostThreatening != NpcGlobalVariables.DefaultInitialVector)
            {
                avoidance = ahead - mostThreatening;

                avoidance = avoidance.normalized;
            }
            else
            {
                avoidance = Vector3.zero;
            }

            return avoidance;
        }
        
   
        private Vector3 findMostThreateningObstacle(Vector3 ahead, NPCManager npc) {
            
            Ray ray = new Ray(npc.transform.position, ahead);
            float maxDistance = ahead.magnitude;

            RaycastHit[] hits = Physics.SphereCastAll(ray, 0.3f, 1f, filter.mask);
            
            if (hits.Length > 0)
            {
                return hits[0].transform.position;
            }

            return NpcGlobalVariables.DefaultInitialVector;
        }
    }
    
}