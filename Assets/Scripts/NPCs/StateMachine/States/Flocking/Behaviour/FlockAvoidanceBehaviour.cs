using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking.Behaviour
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/FlockAvoidance")]

//Move agent away from the neighbour agents
    public class FlockAvoidanceBehaviour : FilteredFlockBehavior
    {
        public override Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock)
        {
            Vector3 avoidanceMove = Vector3.zero;
            int nAvoid = 0;

            var filteredContext = FilterContext(context, npc);
            
            if (filteredContext.Count == 0)
            {
                return Vector3.zero;
            }

            foreach (Transform item in filteredContext)
            {
                nAvoid++;
                avoidanceMove += (npc.transform.position - item.position);
            }

            if (nAvoid > 0)
            {
                avoidanceMove /= nAvoid;
            }

            return avoidanceMove;
        }

        private List<Transform> FilterContext(List<Transform> context, NPCManager npc)
        {
            List<Transform> filteredContext = (filter == null) ? context : filter.Filter(npc, context);

            filteredContext = (filter == null)
                ? filteredContext
                : RemoveElementFromContextByName(filteredContext, "Ground");

            filteredContext = (filter == null)
                ? filteredContext
                : RemoveElementFromContextByName(filteredContext, "RightHand");

            return filter == null ? filteredContext : RemoveItSelfFromContext(filteredContext, npc);
        }
    }
}