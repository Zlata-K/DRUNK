using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking.Behaviour
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]

//Align the rotation of the agent according to the neighbour agents
    public class AlignmentBehaviour : FilteredFlockBehavior
    {
        public override Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock)
        {
            Vector3 alignmentMove = Vector3.zero;
            var filteredContext = FilterContext(npc, context);
        
            if(filteredContext.Count == 0)
                return Vector3.zero;
        
            foreach (Transform item in filteredContext)
            {
                alignmentMove += item.transform.forward;
            }

            alignmentMove /= filteredContext.Count;
        
            return alignmentMove;
        }
        
        private List<Transform> FilterContext(NPCManager npc, List<Transform> context)
        {
            List<Transform> filteredContext = (filter == null) ? context : filter.Filter(npc, context);
            filteredContext = filter == null
                ? filteredContext
                : RemoveElementFromContextByName(filteredContext, "RightHand");

            return filter == null ? filteredContext : RemoveItSelfFromContext(filteredContext, npc);
        }
    }
}
